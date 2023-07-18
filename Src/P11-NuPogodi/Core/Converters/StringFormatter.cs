using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Win8.Core.Converters
{
    /// <summary>
    /// Formatter used for formatting string using CurrentCulture and 
    /// specified format in Databinding.
    /// </summary>
    public class StringFormatter : IValueConverter
    {
        /// <summary>
        /// Format to be used in the converter. If null or empty, unchanged value is returned.
        /// </summary>
        public string Format { get; set; }


        public object Convert(object value, Type targetType, object parameter, 
            string language)
        {

            if (string.IsNullOrEmpty(Format)) return value;

            return string.Format(CultureInfo.CurrentCulture, Format, value).Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
