// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullLoggingService.cs" company="XamlNinja">
//   2011 Richard Griffin and Ollie Riches
// </copyright>
// <summary>
//   Null logging service - several service require an implementation of the ILog interface, they are initialised
//   with this version.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Win8.Core.Tasks.Logging
{
    /// <summary>
    /// Null logging service - several services require an implementation of the ILog interface, they are initialised
    /// with this version.
    /// </summary>
    public sealed class NullLoggingService : ILogManager
    {
        /// <summary>
        /// Event fired when log file has been modified - this implementation does nothing.
        /// </summary>
        public event EventHandler LogModified
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Writes a formatted message to the log with the arguments- this implementation does nothing.
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
            return this;
        }

        /// <summary>
        /// Writes a message and exception to the log - this implementation does nothing.
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
            return this;
        }

        /// <summary>
        /// Gets the path to the log file - this implementation returns a null.
        /// </summary>
        public string LogPath
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets all the current message in the file - this implementation returns an empty.
        /// </summary>
        public IEnumerable<string> Messages
        {
            get { return Enumerable.Empty<string>(); }
        }

        /// <summary>
        /// Enables the logging service - this implementation does nothing.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Enable()
        {
            return this;
        }

        /// <summary>
        /// Disables the logging service - this implementation does nothing.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Disable()
        {
            return this;
        }

        /// <summary>
        /// Clears the contents of the log file - this implementation does nothing.
        /// </summary>
        /// <returns>
        /// Returns the instance of the logging service - fluent interface style.
        /// </returns>
        public ILogManager Clear()
        {
            return this;
        }
    }
}