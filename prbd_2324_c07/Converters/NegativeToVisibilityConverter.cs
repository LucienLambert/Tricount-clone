using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace prbd_2324_c07.Converters
{
    public class NegativeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is double number) {
                return number < 0 ? Visibility.Visible : Visibility.Collapsed;
            } else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}
