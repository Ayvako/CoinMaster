using CoinMaster.Client.Services;
using System.ComponentModel;

namespace CoinMaster.Client.ViewModels;

public class PeriodOption : INotifyPropertyChanged, IDisposable
{
    private readonly string _key;

    public string Value { get; }

    public string Label => LocalizationService.Get(_key);

    public PeriodOption(string key, string value)
    {
        _key = key;
        Value = value;
        LocalizationService.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged() =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Label)));

    public void Dispose() =>
        LocalizationService.LanguageChanged -= OnLanguageChanged;

    public event PropertyChangedEventHandler? PropertyChanged;
}