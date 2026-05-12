using System.Windows;

namespace CoinMaster.Client.Services;

public static class ThemeManager
{
    private const string DarkThemePath = "Themes/DarkTheme.xaml";

    private const string LightThemePath = "Themes/LightTheme.xaml";

    private static bool isDark = true;

    public static bool IsDark => isDark;

    public static event EventHandler<bool>? ThemeChanged;

    public static void Apply(bool isDark)
    {
        if (ThemeManager.isDark == isDark) return;

        ThemeManager.isDark = isDark;
        SwapDictionary(
            isDark ? LightThemePath : DarkThemePath,
            isDark ? DarkThemePath : LightThemePath);

        ThemeChanged?.Invoke(null, isDark);
    }

    public static void Toggle() => Apply(!isDark);

    private static void SwapDictionary(string removePath, string addPath)
    {
        var app = Application.Current;
        if (app == null) return;

        var mergedDicts = app.Resources.MergedDictionaries;

        var oldDict = mergedDicts.FirstOrDefault(d =>
            d.Source != null && d.Source.OriginalString.EndsWith(removePath, StringComparison.OrdinalIgnoreCase));

        if (oldDict != null)
            mergedDicts.Remove(oldDict);

        mergedDicts.Add(new ResourceDictionary
        {
            Source = new Uri(addPath, UriKind.Relative)
        });
    }

    public static void Initialize(bool? forceDark = null)
    {
        bool prefersDark = forceDark ?? DetectOsDarkMode();
        isDark = !prefersDark;
        Apply(prefersDark);
    }

    private static bool DetectOsDarkMode()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key?.GetValue("AppsUseLightTheme") is int value)
                return value == 0;
        }
        catch { }
        return true;
    }
}