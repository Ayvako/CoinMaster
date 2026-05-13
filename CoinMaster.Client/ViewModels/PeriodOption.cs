namespace CoinMaster.Client.ViewModels;

using System.ComponentModel;
using CoinMaster.Client.Services;

public class PeriodOption : INotifyPropertyChanged, IDisposable
{
    private readonly string key;

    private bool disposed = false;

    public PeriodOption(string key, string value)
    {
        this.key = key;
        this.Value = value;
        LocalizationService.LanguageChanged += this.OnLanguageChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Value { get; }

    public string Label => LocalizationService.Get(this.key);

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            LocalizationService.LanguageChanged -= this.OnLanguageChanged;
        }

        this.disposed = true;
    }

    private void OnLanguageChanged() =>
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Label)));
}