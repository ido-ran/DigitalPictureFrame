using System;
using System.Globalization;
using System.Windows.Data;

namespace DigitalFrame.Module.Time.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string)
            {
                return string.Format(parameter as string, value);
            }

            return parameter + " " + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}