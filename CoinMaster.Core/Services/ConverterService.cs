using CoinMaster.Core.Interfaces;

namespace CoinMaster.Core.Services;

public class ConverterService : IConverterService
{
    private readonly IConverterProvider converterProvider;

    public ConverterService(IConverterProvider converterProvider)
    {
        this.converterProvider = converterProvider;
    }

    public async Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount)
    {
        return await converterProvider.ConvertAsync(fromCoinId, toCoinId, amount);
    }
}