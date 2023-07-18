using System;
using System.Globalization;

namespace Win8.Core.MultiBinding
{
    /// <summary>
    /// see: http://msdn.microsoft.com/en-us/library/system.windows.data.imultivalueconverter.aspx
    /// </summary>
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
