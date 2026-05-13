namespace CoinMaster.Core.Services;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

public class MarketService : IMarketService
{
    private readonly IMarketProvider marketProvider;

    public MarketService(IMarketProvider marketProvider)
    {
        this.marketProvider = marketProvider;
    }

    public Task<List<Market>> GetMarketsAsync(string coinId, int limit = 10)
    {
        return this.marketProvider.GetMarketsAsync(coinId, limit);
    }
}