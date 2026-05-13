using CoinMaster.Core.Entities;

namespace CoinMaster.Core.Interfaces;

public interface IMarketService
{
    Task<List<Market>> GetMarketsAsync(string coinId, int limit = 10);
}