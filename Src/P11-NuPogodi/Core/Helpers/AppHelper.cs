using System;
using System.Diagnostics;
using System.Windows;
//using System.Windows.Media;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
//using Microsoft.Phone.Info;
//using Microsoft.Xna.Framework.GamerServices;

namespace Win8.Core.Helpers
{
    /// <summary>
    /// Class for accessing global application properties.
    /// </summary>
    public class AppHelper
    {
        /// <summary>
        /// Reset the cached values of <see cref="IsTrial"/> and <see cref="IsBlackTheme"/>
        /// </summary>
        public static void Reset(AppLaunchType launchType)
        {
            isBlackTheme = null;
            isTrial = null;
        }

        /// <summary>
        /// Return true if pohone uses Black color theme.
        /// </summary>
        public static bool IsBlackTheme
        {
            get
            {
                if (!isBlackTheme.HasValue)
                {
                    SolidColorBrush bg = default;//Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
                    isBlackTheme = bg != null && bg.Color == Colors.Black;
                }
                return isBlackTheme.Value;
            }
        }
        private static bool? isBlackTheme;

        /// <summary>
        /// Flag indicating if game is running in trial mode or not.
        /// </summary>
        public static bool IsTrial
        {
            get
            {
                if (!isTrial.HasValue)
                {
#if DEBUG
                    // NOTE trial mode emulation is broken on WP8, so we use fixed trial mode in Debug mode
                    isTrial = false;//true;
#else
                    isTrial = false;//Guide.IsTrialMode;
#endif
                }
                return isTrial.Value;
            }
        }
        private static bool? isTrial;

        /// <summary>
        /// Flag if device is Low-memory Tango device or not.
        /// </summary>
        public static bool IsLowMemDevice
        {
            get
            {
                if (!isLowMemDevice.HasValue)
                {
                    try
                    {
                        // check the working set limit 
                        long result = 0;//(long)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");
                        isLowMemDevice = false;//result < 94371840L;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // OS does not support this call => indicates a 512 MB device
                        isLowMemDevice = false;
                    }
                }
                return isLowMemDevice.Value;
            }
        }
        private static bool? isLowMemDevice;

        /// <summary>
        /// Flag indicating if app was downloaded from marketplace and sideloaded.
        /// </summary>
        public static bool IsSideloaded
        {
            get
            {
                if (!isSideloaded.HasValue)
                {
                    try
                    {
                        // skip the check during development
                        if (Debugger.IsAttached)
                        {
                            isSideloaded = false;
                        }
                        else
                        {
                            // the WMAppPRHeader.xml file will be added during AppHub certification. App with this file cannot be sideloaded
                            const string fl = "WMAppPRHeader.xml";
                            XDocument doc = XDocument.Load(fl);
                            // the file is present, all is OK
                            isSideloaded = false;
                        }
                    }
                    catch (Exception)
                    {
                        // app is sideloaded, this file is missing or is empty
                        isSideloaded = true;
                    }
                }
                return isSideloaded.Value;
            }
        }
        private static bool? isSideloaded;
    }
}