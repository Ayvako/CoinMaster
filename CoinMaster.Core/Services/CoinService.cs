using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class CoinService : ICoinService
{
    private readonly ICoinProvider client;

    private readonly IMarketProvider marketProvider;

    public CoinService(ICoinProvider client, IMarketProvider marketProvider)
    {
        this.client = client;
        this.marketProvider = marketProvider;
    }

    public async Task<Coin> GetDetailsAsync(string id)
    {
        var coin = await client.GetDetailsAsync(id);
        coin.Markets = marketProvider.GetMarketsAsync(id).Result;
        return coin;
    }

    public Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
        => client.GetTopCoinsAsync(limit);
}