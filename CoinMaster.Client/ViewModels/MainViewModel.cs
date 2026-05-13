namespace CoinMaster.Client.ViewModels;

using System.Collections.ObjectModel;
using System.Diagnostics;
using CoinMaster.Client.Services;
using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
            var coins = await this.coinService.GetTopCoinsAsync(10);
            this.TopCoins = new ObservableCollection<Coin>(coins);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Помилка завантаження: {ex.Message}");
        }
    }

    public async Task InitializeAsync()
    {
        if (this.TopCoins.Any())
        {
            return;
        }

        await this.LoadTopCoinsAsync();
    }

    public async Task SelectCoinAsync(Coin coin)
    {
        await this.navigation.NavigateToAsync<CoinDetailsViewModel>(vm => vm.Load(coin));
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(this.SearchQuery))
        {
            await this.LoadTopCoinsAsync();
            return;
        }

        var result = await this.coinService.SearchCoinsAsync(this.SearchQuery);
        if (result == null)
        {
            return;
        }

        this.TopCoins = new ObservableCollection<Coin>(result);
    }
}