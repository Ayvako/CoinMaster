namespace CoinMaster.Core.Services;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class CoinService : ICoinService
{
    private readonly ICoinProvider coinProvider;

    private readonly IMarketService marketService;

    private readonly IChartService chartService;

    private readonly IMemoryCache cache;

    public CoinService(ICoinProvider coinProvider, IMarketService marketService, IChartService chartService, IMemoryCache cache)
    {
        this.coinProvider = coinProvider;
        this.marketService = marketService;
        this.chartService = chartService;
        this.cache = cache;
    }

    public async Task<Coin?> GetDetailsAsync(string id, string days = "7")
    {
        string coinCacheKey = $"coin_base_{id}";
        var coin = await this.cache.GetOrCreateAsync(coinCacheKey, async entry =>
        {
            var result = await this.coinProvider.GetDetailsAsync(id);

            if (result == null)
            {
                entry.SetAbsoluteExpiration(TimeSpan.Zero);
                return null;
            }

            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return result;
        });

        if (coin == null)
        {
            return null;
        }

        var marketsTask = this.marketService.GetMarketsAsync(id);
        var ohlcTask = this.chartService.GetOhlcAsync(id, days);

        await Task.WhenAll(marketsTask, ohlcTask);

        coin.Markets = (await marketsTask)?.Take(10).ToList() ?? [];
        coin.OhlcData = await ohlcTask ?? [];
        return coin;
    }

    public Task<List<Coin>?> GetTopCoinsAsync(int limit = 10)
    {
        string cacheKey = $"top_coins_{limit}";

        return this.cache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            return this.coinProvider.GetTopCoinsAsync(limit);
        });
    }

    public Task<List<Coin>> SearchCoinsAsync(string searchQuery)
        => this.coinProvider.SearchCoinsAsync(searchQuery);
}