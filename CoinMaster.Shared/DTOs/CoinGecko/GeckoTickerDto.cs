using System.Text.Json.Serialization;

namespace CoinMaster.Shared.DTOs.CoinGecko;

public class GeckoTickerDto
{
    [JsonPropertyName("market")]
    public GeckoMarketDto Market { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; }

    [JsonPropertyName("target")]

    public string Target { get; set; }

    [JsonPropertyName("volume")]
    public decimal Volume { get; set; }

    [JsonPropertyName("last")]
    public decimal LastPrice { get; set; }

    [JsonPropertyName("trade_url")]
    public string TradeUrl { get; set; }
}