namespace CoinMaster.Core.Services;

using CoinMaster.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class ConverterService : IConverterService
{
    private readonly IConverterProvider converterProvider;

    private readonly IMemoryCache cache;

    public ConverterService(IConverterProvider converterProvider, IMemoryCache cache)
    {
        this.converterProvider = converterProvider;
        this.cache = cache;
    }

    public async Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount)
    {
        string cacheKey = $"convert_{fromCoinId}_{toCoinId}_{amount}";

        return await this.cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await this.converterProvider.ConvertAsync(fromCoinId, toCoinId, amount);
        });
    }
}