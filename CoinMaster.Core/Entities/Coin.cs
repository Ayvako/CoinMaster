namespace CoinMaster.Core.Entities;

public class Coin
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Symbol { get; set; }

    public int Rank { get; set; }

    public decimal PriceUsd { get; set; }

    public decimal Change24Hr { get; set; }

    public decimal MarketCapUsd { get; set; }

    public decimal? MaxSupply { get; set; }

    public decimal VolumeUsd24Hr { get; set; }

    public List<Market> Markets { get; set; } = [];

    public List<Candle> OhlcData { get; set; } = [];
}