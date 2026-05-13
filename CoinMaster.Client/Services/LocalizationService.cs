namespace CoinMaster.Client.Services;

using System.Windows;

public static class LocalizationService
{
    public static event Action? LanguageChanged;

    public static string Get(string key) =>
    Application.Current.Resources[key] as string ?? key;

    public static void SetLanguage(string lang)
    {
        var dict = new ResourceDictionary
        {
            Source = new Uri($"Resources/Localization/Strings.{lang}.xaml", UriKind.Relative),
        };
        var old = Application.Current.Resources.MergedDictionaries
            .First(d => d.Source?.OriginalString.Contains("Strings") == true);

        Application.Current.Resources.MergedDictionaries.Remove(old);
        Application.Current.Resources.MergedDictionaries.Add(dict);

        LanguageChanged?.Invoke();
    }
}