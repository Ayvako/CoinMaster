namespace CoinMaster.Core.Services;

using CoinMaster.Core.Interfaces;

public class ConverterService : IConverterService
{
    private readonly IConverterProvider converterProvider;

    public ConverterService(IConverterProvider converterProvider)
    {
        this.converterProvider = converterProvider;
    }

    public async Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount)
    {
        return await this.converterProvider.ConvertAsync(fromCoinId, toCoinId, amount);
    }
}