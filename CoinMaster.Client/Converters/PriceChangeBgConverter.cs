namespace CoinMaster.Client.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

public class PriceChangeBgConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not decimal change)
        {
            return Brushes.Transparent;
        }

        string resourceKey = change >= 0
            ? "PriceUpBgBrush"
            : "PriceDownBgBrush";

        return Application.Current.TryFindResource(resourceKey) as Brush
               ?? Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}