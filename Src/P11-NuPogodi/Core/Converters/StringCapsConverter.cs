using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Win8.Core.Converters
{
    public class StringCapsConverter : IValueConverter
    {
      
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string address = value as string;
            string par = parameter as string;
            if (address == null) return DependencyProperty.UnsetValue;

            switch (par)
            {
                case "upper":
                    return address.ToUpper();
                case "lower":
                case null:
                    return address.ToLower();
                default:
                    return address;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
