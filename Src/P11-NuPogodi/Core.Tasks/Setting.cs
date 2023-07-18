using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace Win8.Core.Tasks
{
    /// <summary>
    /// Encapsulates a key/value pair stored in Isolated Storage ApplicationSettings
    /// </summary>
    /// <typeparam name="T">Type to store, must be serializable.</typeparam>
    public class Setting<T>
    {
        private readonly string name;
        private T value;
        private readonly T defaultValue;
        private bool hasValue;

        public Setting(string name, T defaultValue)
        {
            this.name = name;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// The stored typed value.
        /// </summary>
        public T Value
        {
            get
            {
                // Check for the cached value
                if (!hasValue)
                {
                    try
                    {
                        // Try to get the value from Isolated Storage
                        //RnD
                        if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(name, out value))
                        {
                            // It hasn’t been set yet
                            value = defaultValue;
                            //RnD
                            //IsolatedStorageSettings.ApplicationSettings[name] = value;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Setting name {0}; {1}", name, e);
                        value = defaultValue;
                        //RnD
                       // IsolatedStorageSettings.ApplicationSettings[name] = value;
                    }
                    hasValue = true;
                }
                return value;
            }
            set
            {
                // Save the value to Isolated Storage
                IsolatedStorageSettings.ApplicationSettings[name] = value;
                this.value = value;
                hasValue = true;
            }
        }

        /// <summary>
        /// The default value of the object.
        /// </summary>
        public T DefaultValue
        {
            get { return defaultValue; }
        }

        /// <summary>
        /// Clear the cached value.
        /// </summary>
        public void ForceRefresh()
        {
            hasValue = false;
        }

        /// <summary>
        /// Save cached value to storage
        /// </summary>
        public void ForceSave()
        {
            if (hasValue)
            {
                IsolatedStorageSettings.ApplicationSettings[name] = value;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Return formatted value as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", name, Value);
        }
    }
}
