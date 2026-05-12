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

    private readonly INavigationService navigation;

    [ObservableProperty]
    private ObservableCollection<Coin> topCoins = [];

    [ObservableProperty]
    private string searchQuery = string.Empty;

    public MainViewModel(ICoinService coinService, INavigationService navigation)
    {
        this.coinService = coinService;
        this.navigation = navigation;
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

    public async Task InitializeAsync()
    {
        if (TopCoins.Any()) return;
        await LoadTopCoinsAsync();
    }

    public async Task SelectCoin(Coin coin)
    {
        await navigation.NavigateToAsync<CoinDetailsViewModel>(vm => vm.Load(coin));
    }

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