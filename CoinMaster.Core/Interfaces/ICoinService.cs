using CoinMaster.Core.Entities;

namespace CoinMaster.Core.Interfaces;

public interface ICoinService
{
    Task<List<Coin>> GetTopCoinsAsync(int limit = 10);

    Task<Coin> GetDetailsAsync(string id, string days = "7");

    Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount);
}