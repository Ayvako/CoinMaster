namespace CoinMaster.Core.Interfaces;

using CoinMaster.Core.Entities;

public interface IMarketProvider
{
    Task<List<Market>> GetMarketsAsync(string coinId, int limit = 10);
}