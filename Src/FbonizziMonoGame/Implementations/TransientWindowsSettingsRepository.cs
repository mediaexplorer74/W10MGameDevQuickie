using FbonizziMonoGame.PlatformAbstractions;
using System;
using System.Collections.Generic;

namespace FbonizziMonoGame.Implementations
{
    /// <summary>
    /// An in memory <see cref="ISettingsRepository"/> that is cleaned after the process ends
    /// </summary>
    public class TransientWindowsSettingsRepository : ISettingsRepository
    {
        private readonly Dictionary<string, object> _storage = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetOrSetBool(string key, bool defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (bool)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetOrSetInt(string key, int defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (int)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetOrSetLong(string key, long defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (long)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetOrSetString(string key, string defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (string)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TimeSpan GetOrSetTimeSpan(string key, TimeSpan defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (TimeSpan)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DateTime GetOrSetDateTime(string key, DateTime defaultValue)
        {
            if (_storage.ContainsKey(key))
            {
                return (DateTime)_storage[key];
            }

            _storage.Add(key, defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetBool(string key, bool value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetInt(string key, int value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetLong(string key, long value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetTimeSpan(string key, TimeSpan value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDateTime(string key, DateTime value)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] = value;
            }
            else
            {
                _storage.Add(key, value);
            }
        }
    }
}
