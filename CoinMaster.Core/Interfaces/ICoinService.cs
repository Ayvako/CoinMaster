using CoinMaster.Core.Entities;

namespace CoinMaster.Core.Interfaces;

public interface ICoinService
{
    Task<List<Coin>> GetTopCoinsAsync(int limit = 10);

    Task<Coin> GetDetailsAsync(string id);
}