namespace CoinMaster.Core.Entities;

public class Market
{
    public string ExchangeName { get; set; } = string.Empty;

    public string Base { get; set; }

    public string Target { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal Volume { get; set; }

    public string? TradeUrl { get; set; }
}