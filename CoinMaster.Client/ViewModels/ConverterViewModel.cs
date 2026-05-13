namespace CoinMaster.Client.ViewModels;

using System.Diagnostics;
using System.Globalization;
using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class ConverterViewModel(ICoinService coinService, IConverterService converterService)
    : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<Coin> allCoins = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    private Coin? fromCoin;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConvertCommand))]
    private Coin? toCoin;

    [ObservableProperty]
    private string fromAmount = "1";

    [ObservableProperty]
    private string? resultAmount;

    [ObservableProperty]
    private string? exchangeRateHint;

    [ObservableProperty]
    private bool hasResult;

    [ObservableProperty]
    private bool isLoading;

    public async Task LoadAsync()
    {
        this.AllCoins = await coinService.GetTopCoinsAsync(250);
        this.FromCoin = this.AllCoins.FirstOrDefault(c => c.Symbol == "BTC") ?? this.AllCoins.FirstOrDefault();
        this.ToCoin = this.AllCoins.FirstOrDefault(c => c.Symbol == "ETH") ?? this.AllCoins.Skip(1).FirstOrDefault();
    }

    [RelayCommand]
    private void Swap()
    {
        (this.FromCoin, this.ToCoin) = (this.ToCoin, this.FromCoin);
        if (this.HasResult && this.ResultAmount is not null)
        {
            this.FromAmount = this.ResultAmount;
        }

        this.HasResult = false;
        this.ResultAmount = null;
        this.ExchangeRateHint = null;
    }

    private bool CanConvert() =>
        this.FromCoin is not null && this.ToCoin is not null &&
        decimal.TryParse(this.FromAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) && v > 0;

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private async Task Convert()
    {
        if (!decimal.TryParse(this.FromAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
        {
            return;
        }

        this.IsLoading = true;
        this.HasResult = false;
        try
        {
            var result = await converterService.ConvertAsync(this.FromCoin!.Id, this.ToCoin!.Id, amount);
            this.ResultAmount = result >= 1 ? result.ToString("N4") : result.ToString("G8");

            var rate = this.FromCoin.PriceUsd / this.ToCoin.PriceUsd;
            this.ExchangeRateHint = $"1 {this.FromCoin.Symbol} = {(rate >= 1 ? rate.ToString("N4") : rate.ToString("G6"))} {this.ToCoin.Symbol}  •  ${this.FromCoin.PriceUsd:N2}";
            this.HasResult = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            this.IsLoading = false;
        }
    }

    partial void OnFromAmountChanged(string value) => ConvertCommand.NotifyCanExecuteChanged();
}