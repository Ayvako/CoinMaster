using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CoinMaster.Infrastructure.ApiClients.Base;
using CoinMaster.Infrastructure.Mapping;
using CoinMaster.Shared.DTOs.CoinCap;

namespace CoinMaster.Infrastructure.ApiClients.CoinCap;

public class CoinCapClient : BaseApiClient, ICoinProvider
{
    public CoinCapClient(HttpClient httpClient)
        : base(httpClient) { }

    public async Task<Coin> GetDetailsAsync(string id)
    {
        var response = await GetAsync<CoinCapSingleResponse>($"assets/{id}");

        return response?.Data != null ? DtoMapper.ToCoin(response.Data) : new Coin();
    }

    public async Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
    {
        var query = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString()
        };
        var response = await GetAsync<CoinCapResponse>("assets", queryParams: query);

        return response?.Data?.Select(DtoMapper.ToCoin).ToList() ?? [];
    }
}