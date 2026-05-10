namespace CoinMaster.Shared.DTOs.CoinGecko;

public class CoinGeckoTickersResponse
{
    public string Name { get; set; }

    public List<GeckoTickerDto> Tickers { get; set; }
}