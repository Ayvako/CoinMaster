using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoinMaster.Infrastructure.ApiClients.Base;

public abstract class BaseApiClient : IApiClient
{
    private readonly HttpClient httpClient;

    private const int MaxAttempts = 3;

    protected BaseApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default) => GetAsync<T>(endpoint, null, ct);

    public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams, CancellationToken ct = default)
    {
        var url = BuildUrl(endpoint, queryParams);

        for (int attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync(ct);
                    return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, ct);
                }

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (attempt == MaxAttempts) throw new HttpRequestException($"Rate limit exceeded after {MaxAttempts} attempts: {url}");

                    var delay = GetRetryAfterDelay(response, attempt);
                    await Task.Delay(delay, ct);
                    continue;
                }

                if ((int)response.StatusCode >= 500)
                {
                    if (attempt == MaxAttempts) response.EnsureSuccessStatusCode();

                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), ct);
                    continue;
                }

                if ((int)response.StatusCode >= 400)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    throw new HttpRequestException($"Request failed with status code {(int)response.StatusCode} ({response.ReasonPhrase}): {errorContent}");
                }

                response.EnsureSuccessStatusCode();
            }

            catch (HttpRequestException) when (attempt < MaxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), ct);
            }
        }

        throw new InvalidOperationException($"Unreachable code for {url}");
    }

    private static TimeSpan GetRetryAfterDelay(HttpResponseMessage response, int attempt)
    {
        var retryAfter = response.Headers.RetryAfter;

        if (retryAfter?.Delta.HasValue == true)
        {
            return retryAfter.Delta.Value;
        }

        if (retryAfter?.Date.HasValue == true)
        {
            var delay = retryAfter.Date.Value - DateTimeOffset.UtcNow;
            return delay > TimeSpan.Zero ? delay : TimeSpan.FromSeconds(1);
        }

        return TimeSpan.FromSeconds(attempt * 2);
    }

    private static string BuildUrl(string endpoint, Dictionary<string, string>? queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
        {
            return endpoint;
        }

        var query = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

        return $"{endpoint}?{query}";
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };
}