namespace CoinMaster.Core.Interfaces;

public interface IConverterProvider
{
    Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount);
}