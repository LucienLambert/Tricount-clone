using prbd_2324_c07.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace prbd_2324_c07.Converters
{
    public class ObjectToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is ObservableCollection<OperationCardViewModel> op) {
                return op == null ? Visibility.Visible : Visibility.Collapsed;
            } else {
                return Visibility.Visible;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}
