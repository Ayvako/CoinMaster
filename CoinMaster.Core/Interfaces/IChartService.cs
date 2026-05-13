namespace CoinMaster.Core.Interfaces;

using CoinMaster.Core.Entities;

public interface IChartService
{
    Task<List<Candle>> GetOhlcAsync(string coinId, string days = "7", string currency = "usd");
}