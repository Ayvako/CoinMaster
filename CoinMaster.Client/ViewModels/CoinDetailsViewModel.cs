using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinMaster.Client.ViewModels;

public partial class CoinDetailsViewModel : ObservableObject
{

    private readonly ICoinService coinService;

    private readonly IChartService chartService;

    private string coinId = string.Empty;

    public CoinDetailsViewModel(ICoinService coinService, IChartService chartService)
    {
        this.coinService = coinService;
        this.chartService = chartService;
    }

    [ObservableProperty]
    private Coin? coin;

    [ObservableProperty]
    private List<Candle> candles = [];

    [ObservableProperty]
    private bool isLoadingChart;

    public List<PeriodOption> Periods { get; } =
    [
        new("1Д",  "1"),
        new("7Д",  "7"),
        new("14Д", "14"),
        new("1М", "30"),
        new("3М", "90"),
        new("6М", "180"),
        new("1Г",  "365"),
    ];

    [ObservableProperty]
    private PeriodOption selectedPeriod = new("7Д", "7");

    [RelayCommand]
    private async Task SelectPeriod(PeriodOption period)
    {
        SelectedPeriod = period;
        await LoadCandlesAsync();
    }

    public async Task Load(Coin selected)
    {
        coinId = selected.Id;

        Coin = selected;

        Coin = await coinService.GetDetailsAsync(selected.Id);

        if (Coin.OhlcData is { Count: > 0 })
            Candles = Coin.OhlcData;
        else
            await LoadCandlesAsync();

    }

    private async Task LoadCandlesAsync()
    {
        if (string.IsNullOrEmpty(coinId)) return;

        IsLoadingChart = true;
        try
        {
            Candles = await chartService.GetOhlcAsync(coinId, SelectedPeriod.Days);
        }
        finally
        {
            IsLoadingChart = false;
        }
    }
}