using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinMaster.Shared.DTOs.CoinCap;

public class CoinCapDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("rank")]
    public string Rank { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("priceUsd")]
    public string PriceUsd { get; set; }

    [JsonPropertyName("changePercent24Hr")]
    public string ChangePercent24Hr { get; set; }

    [JsonPropertyName("marketCapUsd")]
    public string MarketCapUsd { get; set; }

    [JsonPropertyName("volumeUsd24Hr")]
    public string VolumeUsd24Hr { get; set; }

    [JsonPropertyName("supply")]
    public string Supply { get; set; }

    [JsonPropertyName("maxSupply")]
    public string? MaxSupply { get; set; }

    [JsonPropertyName("vwap24Hr")]
    public string Vwap24Hr { get; set; }

    [JsonPropertyName("explorer")]
    public string? Explorer { get; set; }

    [JsonPropertyName("tokens")]
    public Dictionary<string, List<string>> Tokens { get; set; } = [];
}