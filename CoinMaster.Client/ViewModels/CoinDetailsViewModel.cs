namespace CoinMaster.Client.ViewModels;

using CoinMaster.Core.Entities;
using CoinMaster.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class CoinDetailsViewModel : ObservableObject
{
    private readonly ICoinService coinService;

    private readonly IChartService chartService;

    private string coinId = string.Empty;

    [ObservableProperty]
    private Coin? coin;

    [ObservableProperty]
    private List<Candle> candles = [];

    [ObservableProperty]
    private bool isLoadingChart;

    [ObservableProperty]
    private PeriodOption selectedPeriod;

    public CoinDetailsViewModel(ICoinService coinService, IChartService chartService)
    {
        this.coinService = coinService;
        this.chartService = chartService;
        this.selectedPeriod = this.Periods[1];
    }

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

    public async Task Load(Coin selected)
    {
        this.coinId = selected.Id;

        this.Coin = selected;

        this.Coin = await this.coinService.GetDetailsAsync(selected.Id);

        if (this.Coin.OhlcData is { Count: > 0 })
        {
            this.Candles = this.Coin.OhlcData;
        }
        else
        {
            await this.LoadCandlesAsync();
        }
    }

    [RelayCommand]
    private async Task SelectPeriod(PeriodOption period)
    {
        this.SelectedPeriod = period;
        await this.LoadCandlesAsync();
    }

    private async Task LoadCandlesAsync()
    {
        if (string.IsNullOrEmpty(this.coinId))
        {
            return;
        }

        this.IsLoadingChart = true;
        try
        {
            this.Candles = await this.chartService.GetOhlcAsync(this.coinId, this.SelectedPeriod.Value);
        }
        finally
        {
            this.IsLoadingChart = false;
        }
    }
}