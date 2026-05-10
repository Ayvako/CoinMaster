using CoinMaster.Core.Entities;
using CoinMaster.Shared.DTOs.CoinCap;
using CoinMaster.Shared.DTOs.CoinGecko;
using System.Globalization;

namespace CoinMaster.Infrastructure.Mapping;

public static class DtoMapper
{
    public static Coin ToCoin(CoinCapDto dto) => new()
    {
        Id = dto.Id,
        Rank = int.Parse(dto.Rank),
        Symbol = dto.Symbol,
        Name = dto.Name,
        PriceUsd = decimal.Parse(dto.PriceUsd, CultureInfo.InvariantCulture),
        Change24Hr = decimal.Parse(dto.ChangePercent24Hr, CultureInfo.InvariantCulture),
        MarketCapUsd = decimal.Parse(dto.MarketCapUsd, CultureInfo.InvariantCulture),
        VolumeUsd24Hr = decimal.Parse(dto.VolumeUsd24Hr, CultureInfo.InvariantCulture),
        MaxSupply = dto.MaxSupply is null ? null : decimal.Parse(dto.MaxSupply, CultureInfo.InvariantCulture),
    };

    public static Market ToMarket(GeckoTickerDto dto)
    {
        return new Market
        {
            ExchangeName = dto.Market?.Name ?? "Unknown",
            Base = dto.Base ?? "",
            Target = dto.Target ?? "",
            Price = dto.LastPrice,
            Volume = dto.Volume,
            TradeUrl = dto.TradeUrl
        };
    }
}