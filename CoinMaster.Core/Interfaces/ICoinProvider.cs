namespace CoinMaster.Core.Interfaces;

using CoinMaster.Core.Entities;

public interface ICoinProvider
{
    Task<List<Coin>> GetTopCoinsAsync(int limit = 10);

    Task<Coin?> GetDetailsAsync(string id);

    Task<List<Coin>> SearchCoinsAsync(string queryText);
}