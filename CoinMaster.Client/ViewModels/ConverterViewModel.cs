using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Globalization;

namespace CoinMaster.Client.ViewModels;

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
        AllCoins = await coinService.GetTopCoinsAsync(250);
        FromCoin = AllCoins.FirstOrDefault(c => c.Symbol == "BTC") ?? AllCoins.FirstOrDefault();
        ToCoin = AllCoins.FirstOrDefault(c => c.Symbol == "ETH") ?? AllCoins.Skip(1).FirstOrDefault();
    }

    [RelayCommand]
    private void Swap()
    {
        (FromCoin, ToCoin) = (ToCoin, FromCoin);
        if (HasResult && ResultAmount is not null) FromAmount = ResultAmount;
        HasResult = false;
        ResultAmount = null;
        ExchangeRateHint = null;
    }

    private bool CanConvert() =>
        FromCoin is not null && ToCoin is not null &&
        decimal.TryParse(FromAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) && v > 0;

    [RelayCommand(CanExecute = nameof(CanConvert))]
    private async Task Convert()
    {
        if (!decimal.TryParse(FromAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount)) return;

        IsLoading = true;
        HasResult = false;
        try
        {
            var result = await converterService.ConvertAsync(FromCoin!.Id, ToCoin!.Id, amount);
            ResultAmount = result >= 1 ? result.ToString("N4") : result.ToString("G8");

            var rate = FromCoin.PriceUsd / ToCoin.PriceUsd;
            ExchangeRateHint = $"1 {FromCoin.Symbol} = {(rate >= 1 ? rate.ToString("N4") : rate.ToString("G6"))} {ToCoin.Symbol}  •  ${FromCoin.PriceUsd:N2}"; HasResult = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnFromAmountChanged(string value) => ConvertCommand.NotifyCanExecuteChanged();
}