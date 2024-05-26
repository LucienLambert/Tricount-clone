using prbd_2324_c07.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace prbd_2324_c07.Converters;

public class NameConverterCreator : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        var user = value as User;
        var tricount = parameter as Tricount;
        if (user != null && tricount != null) {
            return user.UserId == tricount.CreatorId ? $"{user.FullName} (Creator)" : user.FullName;
        }
        return value;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}

