using System.Globalization;
using System.Windows.Data;

namespace CoinMaster.Client.Converters;

[ValueConversion(typeof(decimal), typeof(string))]
public class CompactPercentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is decimal d || TryConvert(value, out d)))
            return value?.ToString() ?? string.Empty;

        string sign = d >= 0 ? "+" : "-";
        decimal abs = Math.Abs(d);

        return abs switch
        {
            >= 1_000_000_000 => $"{sign}{abs / 1_000_000_000:0.#}B%",
            >= 1_000_000 => $"{sign}{abs / 1_000_000:0.#}M%",
            >= 1_000 => $"{sign}{abs / 1_000:0.##}K%",
            _ => $"{sign}{abs:0.00}%"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;

    private static bool TryConvert(object? value, out decimal result)
    {
        result = 0;
        if (value == null) return false;
        return decimal.TryParse(value.ToString(),
            NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }
}