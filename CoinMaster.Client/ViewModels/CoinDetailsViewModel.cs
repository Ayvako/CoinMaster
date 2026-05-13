using CoinMaster.Client.Services;
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
        selectedPeriod = Periods[1];

    }

    [ObservableProperty]
    private Coin? coin;

    [ObservableProperty]
    private List<Candle> candles = [];

    [ObservableProperty]
    private bool isLoadingChart;

    [ObservableProperty]
    private PeriodOption selectedPeriod;

    public List<PeriodOption> Periods { get; } =
    [
        new("Period.1D",  "1"),
        new("Period.7D",  "7"),
        new("Period.14D", "14"),
        new("Period.1M",  "30"),
        new("Period.3M",  "90"),
        new("Period.6M",  "180"),
        new("Period.1Y",  "365"),
    ];

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
            Candles = await chartService.GetOhlcAsync(coinId, SelectedPeriod.Value);
        }
        finally
        {
            IsLoadingChart = false;
        }
    }
}