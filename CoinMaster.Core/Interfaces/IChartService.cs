using CoinMaster.Core.Entities;

namespace CoinMaster.Core.Interfaces;

public interface IChartService
{
    Task<List<Candle>> GetOhlcAsync(string coinId, string days = "7", string currency = "usd");
}