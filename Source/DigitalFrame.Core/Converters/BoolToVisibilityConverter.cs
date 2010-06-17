using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DigitalFrame.Core.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool booleanValue;
            bool expectedValue = true;

            if (value is string)
            {
                Boolean.TryParse((string)value, out booleanValue);
            }
            else if (value is bool)
            {
                booleanValue = (bool)value;
            }
            else
            {
                return Visibility.Visible;
            }

            if (parameter is string)
            {
                Boolean.TryParse((string)parameter, out expectedValue);
            }
            else if (parameter is bool)
            {
                expectedValue = (bool)parameter;
            }

            return booleanValue == expectedValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
