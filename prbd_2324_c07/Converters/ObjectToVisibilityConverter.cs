using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace prbd_2324_c07.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            // Si string vide/null ou Date par défaut (non définié) ou 0 ou autre objet (null compris)  => Visibility Collapsed
            if (value is string str) {
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
            } else if (value is DateTime dt) {
                return dt == default(DateTime) ? Visibility.Collapsed : Visibility.Visible;
            } else if (value is Double db) { 
                return db == 0 ? Visibility.Collapsed : Visibility.Visible;
            }else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}
