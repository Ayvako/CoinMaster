using CoinMaster.Client.Services;
using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CoinMaster.Client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICoinService coinService;

    [ObservableProperty]
    private ObservableCollection<Coin> topCoins = [];

    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private string themeIcon = "☀";

    public MainViewModel(ICoinService coinService)
    {
        this.coinService = coinService;
        ThemeManager.Initialize();
        UpdateThemeIcon();
        ThemeManager.ThemeChanged += (s, isDark) => UpdateThemeIcon();
    }

    public async Task LoadTopCoinsAsync()
    {
        try
        {
            var coins = await coinService.GetTopCoinsAsync(10);
            TopCoins = new ObservableCollection<Coin>(coins);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Помилка завантаження: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeManager.Toggle();
        UpdateThemeIcon();
    }

    private void UpdateThemeIcon() => ThemeIcon = ThemeManager.IsDark ? "☀" : "☾";

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await LoadTopCoinsAsync();
            return;
        }

        var result = await coinService.SearchCoinsAsync(SearchQuery);
        if (result == null) return;

        TopCoins = new ObservableCollection<Coin>(result);
    }
}