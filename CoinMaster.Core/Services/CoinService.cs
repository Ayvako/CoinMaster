using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class CoinService : ICoinService
{
    private readonly ICoinProvider client;

    public CoinService(ICoinProvider client)
        => this.client = client;

    public Task<Coin> GetDetailsAsync(string id) =>
        client.GetDetailsAsync(id);

    public Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
        => client.GetTopCoinsAsync(limit);
}