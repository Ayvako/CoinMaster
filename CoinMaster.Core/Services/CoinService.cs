using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class CoinService : ICoinService
{
    private readonly ICoinProvider coinProvider;

    private readonly IMarketProvider marketProvider;

    private readonly IChartProvider chartProvider;

    public CoinService(ICoinProvider coinProvider, IMarketProvider marketProvider, IChartProvider chartProvider)
    {
        this.coinProvider = coinProvider;
        this.marketProvider = marketProvider;
        this.chartProvider = chartProvider;
    }

    public async Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount)
    {
        var fromTask = coinProvider.GetDetailsAsync(fromCoinId);
        var toTask = coinProvider.GetDetailsAsync(toCoinId);

        await Task.WhenAll(fromTask, toTask);

        var fromCoin = fromTask.Result;
        var toCoin = toTask.Result;

        if (fromCoin == null || toCoin == null)
            throw new InvalidOperationException($"Coin not found");

        if (toCoin.PriceUsd == 0)
            throw new InvalidOperationException($"{toCoinId} has zero price");

        return amount * (fromCoin.PriceUsd / toCoin.PriceUsd);
    }

    public async Task<Coin> GetDetailsAsync(string id, string days = "7")
    {
        var coinTask = coinProvider.GetDetailsAsync(id);
        var marketsTask = marketProvider.GetMarketsAsync(id);
        var ohlcTask = chartProvider.GetOhlcAsync(id, days);

        await Task.WhenAll(coinTask, marketsTask, ohlcTask);
        var coin = coinTask.Result;

        coin.Markets = marketsTask.Result?.Take(10).ToList() ?? [];
        coin.OhlcData = ohlcTask.Result ?? [];
        return coin;
    }

    public Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
        => coinProvider.GetTopCoinsAsync(limit);

    public Task<List<Coin>> SearchCoinsAsync(string searchQuery)
        => coinProvider.SearchCoinsAsync(searchQuery);

}