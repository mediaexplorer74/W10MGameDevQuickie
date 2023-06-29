using FbonizziMonoGame.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FbonizziMonoGame.Implementations
{
    /// <summary>
    /// An <see cref="ISettingsRepository"/> that saves a file in the format 'key:value\n'
    /// </summary>
    public class FileWindowsSettingsRepository : ISettingsRepository
    {
        private readonly string _fileName;
        private Dictionary<string, string> _storage;

        /// <summary>
        /// An <see cref="ISettingsRepository"/> that saves a file in the format 'key:value\n'
        /// </summary>
        /// <param name="settingsFilePath">The settings file path</param>
        public FileWindowsSettingsRepository(string settingsFilePath)
        {
            if (string.IsNullOrWhiteSpace(settingsFilePath))
            {
                throw new ArgumentNullException(nameof(settingsFilePath));
            }

            _fileName = settingsFilePath;

            if (File.Exists(_fileName))
            {
                Deserialize(File.ReadAllLines(_fileName));
            }
            else
            {
                _storage = new Dictionary<string, string>();
            }
        }

        private string Serialize()
        {
            var serializedString = new StringBuilder();
            
            foreach (var setting in _storage)
            {
                serializedString.Append(setting.Key).Append(':').Append(setting.Value).Append(Environment.NewLine);
            }

            return serializedString.ToString();
        }

        private void Deserialize(string[] data)
        {
            _storage = new Dictionary<string, string>();

            foreach (var setting in data)
            {
                var splittedSetting = setting.Split(':');
                var key = splittedSetting[0];
                var value = splittedSetting[1];

                _storage.Add(key, value);
            }
        }

        private void Save()
            => File.WriteAllText(_fileName, Serialize());

        /// <summary>
        /// Sets a boolean value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetOrSetBool(string key, bool defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return Convert.ToBoolean(_storage[key] == "1");
            }

            _storage[key] = FromBool(defaultValue);
            Save();
            return defaultValue;
        }

        /// <summary>
        /// Sets a DateTime value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DateTime GetOrSetDateTime(string key, DateTime defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return DateTime.FromBinary(Convert.ToInt64(_storage[key]));
            }

            _storage[key] = defaultValue.ToBinary().ToString();
            Save();
            return defaultValue;
        }

        /// <summary>
        /// Sets an int32 value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetOrSetInt(string key, int defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return Convert.ToInt32(_storage[key]);
            }

            _storage[key] = defaultValue.ToString();
            Save();
            return defaultValue;
        }

        /// <summary>
        /// Sets an int64 value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetOrSetLong(string key, long defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return Convert.ToInt64(_storage[key]);
            }

            _storage[key] = defaultValue.ToString();
            Save();
            return defaultValue;
        }

        /// <summary>
        /// Sets a string value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetOrSetString(string key, string defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return _storage[key];
            }

            _storage[key] = defaultValue;
            Save();
            return defaultValue;
        }

        /// <summary>
        /// Sets a TimeSpan value to settings file if the key is not present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TimeSpan GetOrSetTimeSpan(string key, TimeSpan defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return TimeSpan.FromTicks(GetOrSetLong(key, defaultValue.Ticks));
            }

            SetTimeSpan(key, defaultValue);
            Save();
            return defaultValue;
        }

        private string FromBool(bool value)
            => value ? "1" : "0";

        /// <summary>
        /// Sets a boolean value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetBool(string key, bool value)
        {
            _storage[key] = FromBool(value);
            Save();
        }
        
        /// <summary>
        /// Sets a DateTime value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDateTime(string key, DateTime value)
        {
            _storage[key] = value.ToBinary().ToString();
            Save();
        }

        /// <summary>
        /// Sets an int32 value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetInt(string key, int value)
        {
            _storage[key] = value.ToString();
            Save();
        }

        /// <summary>
        /// Sets an int64 value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetLong(string key, long value)
        {
            _storage[key] = value.ToString();
            Save();
        }

        /// <summary>
        /// Sets a string value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            _storage[key] = value;
            Save();
        }

        /// <summary>
        /// Sets a TimeSpan value to settings file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetTimeSpan(string key, TimeSpan value)
        {
            SetLong(key, value.Ticks);
            Save();
        }
    }
}
