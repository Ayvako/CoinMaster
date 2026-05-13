namespace CoinMaster.Client.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

public class NullToCollapsedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s && !string.IsNullOrWhiteSpace(s)
            ? Visibility.Visible
            : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}