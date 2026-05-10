using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CoinMaster.Infrastructure.ApiClients.Base;
using CoinMaster.Infrastructure.Mapping;
using CoinMaster.Shared.DTOs.CoinGecko;
using Microsoft.Extensions.Configuration;

namespace CoinMaster.Infrastructure.ApiClients.CoinGecko;

public class CoinGeckoClient : BaseApiClient, IMarketProvider
{
    public CoinGeckoClient(HttpClient httpClient, IConfiguration configuration)
    : base(httpClient, configuration["CoinGecko:BaseUrl"], configuration["CoinGecko:ApiKey"]) { }

    public async Task<List<Market>> GetMarketsAsync(string coinId)
    {
        var response = await GetAsync<CoinGeckoTickersResponse>($"coins/{coinId}/tickers");

        if (response?.Tickers == null || response.Tickers.Count == 0)
        {
            return [];
        }

        var markets = response.Tickers
            .Select(DtoMapper.ToMarket)
            .ToList();

        return markets;
    }
}