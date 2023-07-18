using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Win8.Core.Converters
{
    /// <summary>
    /// Converter used for radiobuttons twoway binding with enums.
    /// </summary>
    public class EnumBooleanConverter : IValueConverter
    {      

      
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string parameterString = parameter as string;

            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }
            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            object parameterValue = Enum.Parse(value.GetType(), parameterString, true);
            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            string language)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return Enum.Parse(targetType, parameterString, true);
        }
   
    }
}
