using CoinMaster.Client.ViewModels;
using CoinMaster.Core.Interfaces;
using CoinMaster.Core.Services;
using CoinMaster.Infrastructure.ApiClients.CoinCap;
using CoinMaster.Infrastructure.ApiClients.CoinGecko;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();

        mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();

        mainWindow.Show();
    }

    public static IServiceProvider Services { get; private set; } = null!;

    public static IConfiguration Configuration { get; private set; } = null!;

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddHttpClient<ICoinProvider, CoinCapClient>(client =>
        {
            client.BaseAddress = new Uri(Configuration["CoinCap:BaseUrl"]);
            var apiKey = Configuration["CoinCap:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }
        });

        services.AddHttpClient<CoinGeckoClient>(client =>
        {
            client.BaseAddress = new Uri(Configuration["CoinGecko:BaseUrl"]);
            client.DefaultRequestHeaders.Add("User-Agent", "CoinMasterApp/1.0");

            var apiKey = Configuration["CoinGecko:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Add("x-cg-demo-api-key", apiKey);
            }
        });

        services.AddTransient<IChartProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());
        services.AddTransient<IMarketProvider>(sp => sp.GetRequiredService<CoinGeckoClient>());

        services.AddTransient<ICoinService, CoinService>();

        services.AddTransient<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainViewModel>();

    }
}