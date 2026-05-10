using CoinMaster.Core.Interfaces;
using CoinMaster.Core.Services;
using CoinMaster.Infrastructure.ApiClients.CoinCap;
using CoinMaster.Infrastructure.ApiClients.CoinGecko;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CoinMaster.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        Services = serviceCollection.BuildServiceProvider();
    }

    public static IServiceProvider Services { get; private set; } = null!;

    public static IConfiguration Configuration { get; private set; } = null!;

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddHttpClient<ICoinProvider, CoinCapClient>();

        services.AddHttpClient<CoinGeckoClient>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "CoinMasterApp/1.0");
        });

        services.AddTransient<IChartProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());
        services.AddTransient<IMarketProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());

        services.AddTransient<ICoinService, CoinService>();
    }
}