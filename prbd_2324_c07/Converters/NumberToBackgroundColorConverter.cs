using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace prbd_2324_c07.Converters
{ 
    // Utilisé pour le background des cartes du Main Menu
    public class NumberToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double number) {
                if (number < 0) {
                    return Brushes.AntiqueWhite;
                } else if (number == 0) {
                    return Brushes.LightGray;
                } else {
                    return Brushes.Honeydew;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}