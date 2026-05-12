using System.Globalization;
using System.Windows.Data;

namespace CoinMaster.Client.Converters;

[ValueConversion(typeof(decimal), typeof(string))]
public class PriceConverter : IValueConverter
{
    public string CurrencySymbol { get; set; } = "$";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not decimal price && !TryCoerce(value, out price))
            return value?.ToString() ?? "0.00";

        if (price == 0) return $"{CurrencySymbol}0.00";

        var abs = Math.Abs(price);

        string formatted = abs switch
        {
            >= 10_000m => price.ToString("N0", CultureInfo.CurrentCulture),
            >= 1m => price.ToString("N2", CultureInfo.CurrentCulture),
            >= 0.01m => price.ToString("N4", CultureInfo.CurrentCulture),
            _ => FormatVerySmall(price)
        };

        return $"{CurrencySymbol}{formatted}";
    }

    private static string FormatVerySmall(decimal price)
    {
        double dPrice = (double)Math.Abs(price);
        int leadingZeros = (int)Math.Abs(Math.Floor(Math.Log10(dPrice)));

        int precision = Math.Clamp(leadingZeros + 3, 6, 10);
        return price.ToString($"F{precision}", CultureInfo.CurrentCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();

    private static bool TryCoerce(object? value, out decimal result)
    {
        try
        {
            result = System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            return true;
        }
        catch { result = 0; return false; }
    }
}