namespace CoinMaster.Core.Services;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

public class CoinService : ICoinService
{
    private readonly ICoinProvider coinProvider;

    private readonly IMarketService marketService;

    private readonly IChartService chartService;

    public CoinService(ICoinProvider coinProvider, IMarketService marketService, IChartService chartService)
    {
        this.coinProvider = coinProvider;
        this.marketService = marketService;
        this.chartService = chartService;
    }

    public async Task<Coin> GetDetailsAsync(string id, string days = "7")
    {
        var coinTask = this.coinProvider.GetDetailsAsync(id);
        var marketsTask = this.marketService.GetMarketsAsync(id);
        var ohlcTask = this.chartService.GetOhlcAsync(id, days);

        await Task.WhenAll(coinTask, marketsTask, ohlcTask);
        var coin = coinTask.Result;

        coin.Markets = marketsTask.Result?.Take(10).ToList() ?? [];
        coin.OhlcData = ohlcTask.Result ?? [];
        return coin;
    }

    public Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
        => this.coinProvider.GetTopCoinsAsync(limit);

    public Task<List<Coin>> SearchCoinsAsync(string searchQuery)
        => this.coinProvider.SearchCoinsAsync(searchQuery);
}