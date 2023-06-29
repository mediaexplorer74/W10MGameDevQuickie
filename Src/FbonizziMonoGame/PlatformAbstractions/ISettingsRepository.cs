using System;

namespace FbonizziMonoGame.PlatformAbstractions
{
    /// <summary>
    /// Abstracts each platform settings store
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Gets a string or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetOrSetString(string key, string defaultValue);

        /// <summary>
        /// Sets string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetString(string key, string value);

        /// <summary>
        /// Gets an Int64 or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        long GetOrSetLong(string key, long defaultValue);

        /// <summary>
        /// Sets an Int64
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetLong(string key, long value);

        /// <summary>
        /// Gets an Int32 or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int GetOrSetInt(string key, int defaultValue);

        /// <summary>
        /// Sets an Int32
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetInt(string key, int value);

        /// <summary>
        /// Gets a boolean value or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        bool GetOrSetBool(string key, bool defaultValue);

        /// <summary>
        /// Sets an boolean value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetBool(string key, bool value);

        /// <summary>
        /// Gets a TimeSpan or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        TimeSpan GetOrSetTimeSpan(string key, TimeSpan defaultValue);

        /// <summary>
        /// Sets a TimeSpan
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetTimeSpan(string key, TimeSpan value);

        /// <summary>
        /// Gets a DateTime or sets a value if it doens't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        DateTime GetOrSetDateTime(string key, DateTime defaultValue);

        /// <summary>
        /// Sets a DateTime
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetDateTime(string key, DateTime value);
    }
}
