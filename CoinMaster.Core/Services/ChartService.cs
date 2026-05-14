namespace CoinMaster.Core.Services;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class ChartService : IChartService
{
    private readonly IChartProvider chartProvider;

    private readonly IMemoryCache cache;

    public ChartService(IChartProvider chartProvider, IMemoryCache cache)
    {
        this.chartProvider = chartProvider;
        this.cache = cache;
    }

    public async Task<List<Candle>?> GetOhlcAsync(string coinId, string days = "7", string currency = "usd")
    {
        string cacheKey = $"ohlc_{coinId}_{days}_{currency}";

        return await this.cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await this.chartProvider.GetOhlcAsync(coinId, days, currency);
        });
    }
}