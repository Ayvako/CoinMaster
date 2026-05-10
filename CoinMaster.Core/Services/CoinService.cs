using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class CoinService : ICoinService
{
    private readonly ICoinProvider client;

    private readonly IMarketProvider marketProvider;

    private readonly IChartProvider chartProvider;

    public CoinService(ICoinProvider client, IMarketProvider marketProvider, IChartProvider chartProvider)
    {
        this.client = client;
        this.marketProvider = marketProvider;
        this.chartProvider = chartProvider;
    }

    public async Task<Coin> GetDetailsAsync(string id, string days = "7")
    {
        var coinTask = client.GetDetailsAsync(id);
        var marketsTask = marketProvider.GetMarketsAsync(id);
        var ohlcTask = chartProvider.GetOhlcAsync(id, days);

        await Task.WhenAll(coinTask, marketsTask, ohlcTask);
        var coin = coinTask.Result;

        coin.Markets = marketsTask.Result?.Take(10).ToList() ?? [];
        coin.OhlcData = ohlcTask.Result ?? [];
        return coin;
    }

    public Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
        => client.GetTopCoinsAsync(limit);
}