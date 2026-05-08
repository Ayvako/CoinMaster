using CoinMaster.Core.Entities;

namespace CoinMaster.Infrastructure.ApiClients.CoinCap;

public interface ICoinCapClient
{
    Task<List<Coin>> GetTopCoinsAsync(int limit = 10);
}