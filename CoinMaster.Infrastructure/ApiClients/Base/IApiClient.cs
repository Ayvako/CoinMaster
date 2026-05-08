namespace CoinMaster.Infrastructure.ApiClients.Base;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default);

    Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string> queryParams, CancellationToken ct = default);
}