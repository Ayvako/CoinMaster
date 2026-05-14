namespace CoinMaster.Client;

using System.Windows;
using CoinMaster.Client.Services;
using CoinMaster.Client.ViewModels;
using CoinMaster.Core.Interfaces;
using CoinMaster.Core.Services;
using CoinMaster.Infrastructure.ApiClients.CoinCap;
using CoinMaster.Infrastructure.ApiClients.CoinGecko;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interaction logic for App.xaml.
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

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();

        mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();

        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton(Configuration);
        services.AddHttpClient<CoinCapClient>(client =>
        {
            client.BaseAddress = new Uri(Configuration["CoinCap:BaseUrl"]!);
            var apiKey = Configuration["CoinCap:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }
        });

        services.AddHttpClient<CoinGeckoClient>(client =>
        {
            client.BaseAddress = new Uri(Configuration["CoinGecko:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("User-Agent", "CoinMasterApp/1.0");

            var apiKey = Configuration["CoinGecko:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Add("x-cg-demo-api-key", apiKey);
            }
        });

        services.AddTransient<IChartProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());
        services.AddTransient<IMarketProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());

        services.AddTransient<IConverterProvider>(sp => sp.GetRequiredService<CoinCapClient>());
        services.AddTransient<ICoinProvider>(sp => sp.GetRequiredService<CoinCapClient>());

        services.AddTransient<ICoinService, CoinService>();
        services.AddTransient<IChartService, ChartService>();
        services.AddTransient<IMarketService, MarketService>();
        services.AddTransient<IConverterService, ConverterService>();

        services.AddTransient<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<CoinDetailsViewModel>();
        services.AddTransient<ConverterViewModel>();

        services.AddSingleton<INavigationService, NavigationService>();
    }
}