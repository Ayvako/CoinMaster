using System.Globalization;
using System.Windows.Data;

namespace CoinMaster.Client.Converters;

[ValueConversion(typeof(decimal), typeof(string))]
class CompactConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is decimal d || TryConvert(value, out d)))
            return value?.ToString() ?? string.Empty;

        decimal abs = Math.Abs(d);
        return abs switch
        {
            >= 1_000_000_000 => $"{d / 1_000_000_000:0.#}B",
            >= 1_000_000 => $"{d / 1_000_000:0.#}M",
            >= 1_000 => $"{d / 1_000:0.##}K",
            _ => $"{d:0.00}"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static bool TryConvert(object? value, out decimal result)
    {
        result = 0;
        if (value == null) return false;
        return decimal.TryParse(value.ToString(),
            NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }
}