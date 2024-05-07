using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace prbd_2324_c07.Converters
{
    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double number) {
                if (number < 0) {
                    return Brushes.Red;
                } else if (number == 0) {
                    return Brushes.Black;
                } else {
                    return Brushes.Green;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}