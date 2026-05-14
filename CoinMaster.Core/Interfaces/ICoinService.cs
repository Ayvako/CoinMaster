namespace CoinMaster.Core.Interfaces;

using CoinMaster.Core.Entities;

public interface ICoinService
{
    Task<List<Coin>?> GetTopCoinsAsync(int limit = 10);

    Task<Coin?> GetDetailsAsync(string id, string days = "7");

    Task<List<Coin>> SearchCoinsAsync(string searchQuery);
}