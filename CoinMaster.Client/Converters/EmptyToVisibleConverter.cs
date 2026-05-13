using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace CoinMaster.Client.Converters;

[ValueConversion(typeof(IEnumerable), typeof(Visibility))]
public class EmptyToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool isEmpty = value is not ICollection { Count: > 0 };
        bool invert = parameter is string s && s == "invert";
        return (isEmpty ^ invert) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}