using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CoinMaster.Infrastructure.ApiClients.Base;
using CoinMaster.Infrastructure.Mapping;
using CoinMaster.Shared.DTOs.CoinGecko;
using Microsoft.Extensions.Configuration;

namespace CoinMaster.Infrastructure.ApiClients.CoinGecko;

public class CoinGeckoClient : BaseApiClient, IMarketProvider, IChartProvider
{
    public CoinGeckoClient(HttpClient httpClient, IConfiguration configuration)
    : base(httpClient, configuration["CoinGecko:BaseUrl"], configuration["CoinGecko:ApiKey"]) { }

    public async Task<List<Market>> GetMarketsAsync(string coinId, int limit = 10)
    {
        var query = new Dictionary<string, string>
        {
            ["limit"] = limit.ToString()
        };

        var response = await GetAsync<CoinGeckoTickersResponse>($"coins/{coinId}/tickers", queryParams: query);

        if (response?.Tickers == null || response.Tickers.Count == 0)
        {
            return [];
        }

        var markets = response.Tickers
            .Select(DtoMapper.ToMarket)
            .ToList();

        return markets;
    }

    public async Task<List<Candle>> GetOhlcAsync(string coinId, string days = "7", string currency = "usd")
    {
        var query = new Dictionary<string, string>
        {
            ["vs_currency"] = currency,
            ["days"] = days
        };

        var response = await GetAsync<List<List<decimal>>>($"coins/{coinId}/ohlc", queryParams: query);

        return response?.Select(DtoMapper.ToCandle).ToList() ?? [];
    }
}