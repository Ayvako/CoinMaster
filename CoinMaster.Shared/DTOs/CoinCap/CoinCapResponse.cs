using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinMaster.Shared.DTOs.CoinCap;

public class CoinCapResponse
{
    [JsonPropertyName("data")]
    public List<CoinCapDto> Data { get; set; } = [];

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}