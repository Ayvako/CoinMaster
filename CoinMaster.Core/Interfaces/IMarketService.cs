namespace CoinMaster.Core.Interfaces;

using CoinMaster.Core.Entities;

public interface IMarketService
{
    Task<List<Market>?> GetMarketsAsync(string coinId, int limit = 10);
}