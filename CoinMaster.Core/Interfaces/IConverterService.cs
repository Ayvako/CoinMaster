namespace CoinMaster.Core.Interfaces;

public interface IConverterService
{
    Task<decimal> ConvertAsync(string fromCoinId, string toCoinId, decimal amount);
}