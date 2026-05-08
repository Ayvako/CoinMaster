using CoinMaster.Core.Entities;
using CoinMaster.Infrastructure.ApiClients.Base;
using CoinMaster.Infrastructure.Mapping;
using CoinMaster.Shared.DTOs.CoinCap;
using Microsoft.Extensions.Configuration;

namespace CoinMaster.Infrastructure.ApiClients.CoinCap;

public class CoinCapClient : BaseApiClient, ICoinCapClient
{
    public CoinCapClient(HttpClient httpClient, IConfiguration configuration)
        : base(httpClient, configuration["CoinCap:BaseUrl"], configuration["CoinCap:ApiKey"]) { }

    public async Task<List<Coin>> GetTopCoinsAsync(int limit = 10)
    {
        var query = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString()
        };
        var response = await GetAsync<CoinCapResponse>(
            "assets", queryParams: query);

        return response?.Data?.Select(DtoMapper.ToCoin).ToList() ?? new List<Coin>();
    }
}