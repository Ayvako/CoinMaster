using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class ChartService : IChartService
{
    private readonly IChartProvider chartProvider;

    public ChartService(IChartProvider chartProvider)
    {
        this.chartProvider = chartProvider;
    }

    public Task<List<Candle>> GetOhlcAsync(string coinId, string days = "7", string currency = "usd")
    {
        return chartProvider.GetOhlcAsync(coinId, days, currency);
    }
}