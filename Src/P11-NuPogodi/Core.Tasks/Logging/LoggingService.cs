// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingService.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Logging service - provides a mechanism to diagnostically trace statement from WP7 application, this service
//   can be enabled or disabled as and when required. The service persists to isolated storage in the directory defined
//   by the applicationName passed in on the constructor. The service also provides the ability to clear down the file
//   as and when required.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Logging service - provides a mechanism to diagnostically trace statement from WP7 application, this service 
    /// can be enabled or disabled as and when required. The service persists to isolated storage in the directory defined
    /// by the applicationName passed in on the constructor. The service also provides the ability to clear down the file
    /// as and when required.
    /// IMPORTANT - In debug mode the WriteDiagnostics method will query for user and device properties using the methods,
    /// DeviceExtendedProperties and UserExtendedProperties, these methods affect the status of the application in the
    /// application store.
    /// http://msdn.microsoft.com/en-us/library/microsoft.phone.info.deviceextendedproperties.getvalue(v=VS.92).aspx
    /// These method are compiled out for a release build using ifdef DEBUG statements.
    /// </summary>
    public sealed class LoggingService : ILogManager, IDisposable
    {
        /// <summary>
        /// The failed to write.
        /// </summary>
        private const string failedToWrite = "Failed to write to log.";

        /// <summary>
        /// The failed to enable.
        /// </summary>
        private const string failedToEnable = "Failed to enable logging.";

        /// <summary>
        /// The failed to disable.
        /// </summary>
        private const string failedToDisable = "Failed to disable logging.";

        /// <summary>
        /// The failed to read messages.
        /// </summary>
        private const string failedToReadMessages = "Failed to read messages from file.";

        /// <summary>
        /// The failed to clear.
        /// </summary>
        private const string failedToClear = "Failed to clear log file.";

        /// <summary>
        /// Event fired when log file has been modified.
        /// </summary>
        public event EventHandler LogModified = delegate { };

        /// <summary>
        /// The application name.
        /// </summary>
        private readonly string _applicationName;

        /// <summary>
        /// The log path including the log file name;
        /// </summary>
        private readonly string _logPath;

        /// <summary>
        /// The backlog path including the backlog file name;
        /// </summary>
        private readonly string _backLogPath;

        /// <summary>
        /// The isolated storage log filename.
        /// </summary>
        private const string logFilename = "log.dat";

        /// <summary>
        /// The isolated storage back log filename.
        /// </summary>
        private const string backLogFilename = "backlog.dat";

        /// <summary>
        /// Maximum length of a single log file - 20 kB
        /// </summary>
        private const int maxLogLength = 20000;

        /// <summary>
        /// The message format, date and time is pre appened to the message.
        /// </summary>
        private const string messageFormat = "{0:yyyy-MM-dd HH:mm:ss.ffff} - {1}";

        /// <summary>
        /// Pending message queue - messages are written to the in memory queue and then written to file asynchronously.
        /// </summary>
        private readonly Queue<string> _pendingMessages = new Queue<string>();

        /// <summary>
        /// The sync object to make accessing the log file thread safe.
        /// </summary>
        private readonly object _sync = new object();

        /// <summary>
        /// The isolated storage file.
        /// </summary>
        private IsolatedStorageFile _isolatedStorage;

        /// <summary>
        /// Flag indicating persisting to file is occurring.
        /// </summary>
        private volatile bool _persistingToFile;

        /// <summary>
        /// Flag indicating logging is enabled.
        /// </summary>
        private volatile bool _enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application using the logging service.
        /// </param>
        public LoggingService(string applicationName)
        {
            _applicationName = applicationName;
            _isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

            string lowerApplicationName = _applicationName.ToLowerInvariant();

            if (!_isolatedStorage.DirectoryExists(lowerApplicationName))
            {
                _isolatedStorage.CreateDirectory(lowerApplicationName);
            }

            _logPath = Path.Combine(lowerApplicationName, logFilename);
            _backLogPath = Path.Combine(lowerApplicationName, backLogFilename);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            PersistQueueToFile();

            if (_isolatedStorage != null)
            {
                _isolatedStorage.Dispose();
                _isolatedStorage = null;
            }
        }

        /// <summary>
        /// Gets the path to the log file.
        /// </summary>
        public string LogPath
        {
            get { return _logPath; }
        }

        /// <summary>
        /// Gets all the current message in the file.
        /// </summary>
        public IEnumerable<string> Messages
        {
            get
            {
                try
                {
                    // before retrieving log messages, save all pending to the fil
                    PersistQueueToFile();

                    return ReadFile();
                }
                catch (Exception exn)
                {
                    throw new LoggingException(failedToReadMessages, exn);
                }
            }
        }

        /// <summary>
        /// Enables the logging service.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Enable()
        {
            try
            {
                if (_enabled)
                {
                    return this;
                }

                _enabled = true;

                return this;
            }
            catch (Exception exn)
            {
                throw new LoggingException(failedToEnable, exn);
            }
        }

        /// <summary>
        /// Disables the logging service.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Disable()
        {
            try
            {
                if (!_enabled)
                {
                    return this;
                }

                PersistQueueToFile();

                _enabled = false;

                return this;
            }
            catch (Exception exn)
            {
                throw new LoggingException(failedToDisable, exn);
            }
        }

        /// <summary>
        /// Writes a formatted message to the log with the arguments.
        /// </summary>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="args">
        /// The message argument.
        /// </param>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILog Write(string message, params object[] args)
        {
            try
            {
                string builtMessage = string.Format(messageFormat, DateTime.Now, args == null ? message : string.Format(message, args));
                Debug.WriteLine(builtMessage);

                if (!_enabled)
                {
                    return this;
                }

                Enqueue(new List<string> {builtMessage});

                return this;
            }
            catch (Exception exn)
            {
                throw new LoggingException(failedToWrite, exn);
            }
        }

        /// <summary>
        /// Writes a message and exception to the log.
        /// </summary>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILog Write(string message, Exception exception)
        {
            try
            {
                DateTime now = DateTime.Now;

                string formattedMessage1 = string.Format(messageFormat, now, message);
                Debug.WriteLine(formattedMessage1);

                List<string> messages = new List<string> {formattedMessage1};

                if (exception != null)
                {
                    string formattedMessage2 = string.Format(messageFormat, now, exception.Message);
                    string formattedMessage3 = string.Format(messageFormat, now, exception);
                    Debug.WriteLine(formattedMessage2);
                    Debug.WriteLine(formattedMessage3);

                    messages.Add(formattedMessage2);
                    messages.Add(formattedMessage3);
                }

                if (!_enabled)
                {
                    return this;
                }

                Enqueue(messages);

                return this;
            }
            catch (Exception exn)
            {
                throw new LoggingException(failedToWrite, exn);
            }
        }

        /// <summary>
        /// Clears the contents of the log file.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Clear()
        {
            try
            {
                lock (_sync)
                {
                    _pendingMessages.Clear();
                    ClearFile();
                }

                return this;
            }
            catch (Exception exn)
            {
                throw new LoggingException(failedToClear, exn);
            }
        }

        /// <summary>
        /// Adds messages to the queue for writing to file.
        /// </summary>
        /// <param name="messages">
        /// The messages
        /// </param>
        private void Enqueue(List<string> messages)
        {
            messages.ForEach(m => _pendingMessages.Enqueue(m));
        }

        /// <summary>
        /// The persist queue to file.
        /// </summary>
        private void PersistQueueToFile()
        {
            if (_persistingToFile)
            {
                return;
            }

            lock (_sync)
            {
                _persistingToFile = true;

                List<string> messages = new List<string>();
                while (_pendingMessages.Count != 0)
                {
                    messages.Add(_pendingMessages.Dequeue());
                }

                if (messages.Any())
                {
                    WriteFile(messages);
                }

                _persistingToFile = false;
            }
        }

        /// <summary>
        /// Reads the contents of the log file.
        /// </summary>
        /// <returns>
        /// Returns an enumerable collection of persisted messages.
        /// </returns>
        private IEnumerable<string> ReadFile()
        {
            List<string> messages = new List<string>();
            try
            {
                lock (_sync)
                {
                    // first append the back log
                    AddFileLogs(messages, _backLogPath, _isolatedStorage);
                    // then append the actual log
                    AddFileLogs(messages, _logPath, _isolatedStorage);
                }
            }
            catch (Exception exn)
            {
                Debug.WriteLine("LoggingService: Failed to read file, message - '{0}'.", exn.Message);
            }

            return messages;
        }

        private static void AddFileLogs(List<string> messages, string filePath, IsolatedStorageFile storage)
        {
            using (StreamReader sr = new StreamReader(storage.OpenFile(filePath,
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)))
            {
                while (!sr.EndOfStream)
                {
                    messages.Add(sr.ReadLine());
                }
                sr.Close();
            }
        }

        /// <summary>
        /// Writes messages to the file.
        /// </summary>
        /// <param name="messages">
        /// The messages.
        /// </param>
        private void WriteFile(List<string> messages)
        {
            try
            {
                // try to rotate logs, if the main one is full, after writing to file
                TryRotateLogs();

                using (IsolatedStorageFileStream file = _isolatedStorage.OpenFile(_logPath,
                    FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        messages.ForEach(sw.WriteLine);
                        sw.Close();
                    }
                    file.Close();
                }

                LogModified(this, new EventArgs());
            }
            catch (Exception exn)
            {
                Debug.WriteLine("LoggingService: Failed to write to file, message - '{0}'.", exn.Message);
            }
        }

        /// <summary>
        /// Clears both log files.
        /// </summary>
        private void ClearFile()
        {
            try
            {
                using (new StreamWriter(_isolatedStorage.CreateFile(LogPath))) {}
                using (new StreamWriter(_isolatedStorage.CreateFile(_backLogPath))) {}
            }
            catch (Exception exn)
            {
                Debug.WriteLine("LoggingService: Failed to clear file, message - '{0}'.", exn.Message);
            }
        }

        /// <summary>
        /// If the main log is full, rename it to back log
        /// </summary>
        private void TryRotateLogs()
        {
            bool doNotRotate;

            // test if the mainlog is larger than predefined file size
            using (IsolatedStorageFileStream file = _isolatedStorage.OpenFile(_logPath,
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                doNotRotate = file.Length <= maxLogLength;
                file.Close();
            }
            if (doNotRotate) return;

            // before renaming the file delete manually the target backlog file
            if (_isolatedStorage.FileExists(_backLogPath))
            {
                _isolatedStorage.DeleteFile(_backLogPath);
            }

            // if the file is larger, rename it to backlog file name
            _isolatedStorage.MoveFile(_logPath, _backLogPath);
        }
    }
}