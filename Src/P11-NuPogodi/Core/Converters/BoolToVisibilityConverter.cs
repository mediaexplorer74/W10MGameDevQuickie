using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace Win8.Core.Converters
{
    /// <summary>
    /// Returns <see cref="Visibility.Visible"/>, if the input value is true and 
    /// <see cref="Visibility.Collapsed"/>, if the input value is false.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If set to true, the converter returns inverted results.
        /// </summary>
        public bool VisibleWhenFalse { get; set; }

       
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool)) return DependencyProperty.UnsetValue;
            bool visible = ((bool)value) ^ VisibleWhenFalse;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
