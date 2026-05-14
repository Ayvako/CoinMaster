namespace CoinMaster.Core.Services;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class MarketService : IMarketService
{
    private readonly IMarketProvider marketProvider;

    private readonly IMemoryCache cache;

    public MarketService(IMarketProvider marketProvider, IMemoryCache cache)
    {
        this.marketProvider = marketProvider;
        this.cache = cache;
    }

    public async Task<List<Market>?> GetMarketsAsync(string coinId, int limit = 10)
    {
        string cacheKey = $"market_{coinId}_{limit}";

        return await this.cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await this.marketProvider.GetMarketsAsync(coinId, limit);
        });
    }
}