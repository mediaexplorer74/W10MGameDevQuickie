using GalaSoft.MvvmLight.Ioc;
using System;


namespace Win8.Core.Services
{
    /// <summary>
    /// Helper class for proper initialization of the ViewModelLocator.
    /// </summary>
    public class ViewModelHelper
    {
        public static void SafeRegister<T, T2>()
            where T : class
            where T2 : class, T
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
            {
                SimpleIoc.Default.Register<T, T2>();
            }
        }

        public static void SafeRegister<T>(Func<T> factory)
            where T : class
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
            {
                SimpleIoc.Default.Register(factory);
            }
        }

        public static void SafeRegister<T>()
            where T : class
        {
            if (!SimpleIoc.Default.IsRegistered<T>())
            {
                SimpleIoc.Default.Register<T>();
            }
        }
    }
}
