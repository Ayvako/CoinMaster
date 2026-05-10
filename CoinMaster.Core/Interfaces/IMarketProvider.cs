using CoinMaster.Core.Entities;

namespace CoinMaster.Core.Interfaces;

public interface IMarketProvider
{
    Task<List<Market>> GetMarketsAsync(string coinId, int limit = 10);
}