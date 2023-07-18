using System;
using System.Reflection;

namespace Win8.Core.Tasks.Wp8Interop
{
    public static class Utils
    {
        /// <summary>
        /// Returns true if application is running on Windows Phone 8 device.
        /// </summary>
        public static bool IsWP8
        {
            get { return true; }
        }
        private static readonly Version targetedVersionWP8 = new Version(8, 0);

        /// <summary>
        /// Returns true if application is running on Windows Phone 7.8 device.
        /// </summary>
        public static bool IsWP78
        {
            get
            {
                Version v = new Version(0,1);//Environment.OSVersion.Version;
                return v >= targetedVersionWP78 && v < targetedVersionWP8;
            }
        }
        private static readonly Version targetedVersionWP78 = new Version(7, 10, 8858);

        /// <summary>
        /// Returns true if new WP8 tiles are supported on current device (WP8 or WP7.8)
        /// </summary>
        public static bool IsNewTileSupported
        {
            get { return true; }
        }

        #region internals

        internal static void SetProperty(object instance, string name, object value)
        {
            MethodInfo setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new[] { value });
        }

        internal static object GetProperty(object instance, string name)
        {
            return instance.GetType().GetProperty(name).GetValue(instance,new Object[]{});
        }

        #endregion
    }
}