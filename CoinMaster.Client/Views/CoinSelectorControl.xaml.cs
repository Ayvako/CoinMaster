using CoinMaster.Core.Entities;
using System.Windows;
using System.Windows.Controls;

namespace CoinMaster.Client.Views;

public partial class CoinSelectorControl : UserControl
{
    public CoinSelectorControl() => InitializeComponent();

    public static readonly DependencyProperty AllCoinsProperty =
        DependencyProperty.Register(nameof(AllCoins), typeof(IEnumerable<Coin>),
            typeof(CoinSelectorControl),
            new PropertyMetadata(null, (d, _) => ((CoinSelectorControl)d).ApplyFilter()));

    public static readonly DependencyProperty SelectedCoinProperty =
        DependencyProperty.Register(nameof(SelectedCoin), typeof(Coin),
            typeof(CoinSelectorControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SearchQueryProperty =
        DependencyProperty.Register(nameof(SearchQuery), typeof(string),
            typeof(CoinSelectorControl),
            new PropertyMetadata(string.Empty, (d, _) => ((CoinSelectorControl)d).ApplyFilter()));

    public static readonly DependencyProperty FilteredCoinsProperty =
        DependencyProperty.Register(nameof(FilteredCoins), typeof(IEnumerable<Coin>),
            typeof(CoinSelectorControl));

    public IEnumerable<Coin>? AllCoins
    {
        get => (IEnumerable<Coin>?)GetValue(AllCoinsProperty);
        set => SetValue(AllCoinsProperty, value);
    }

    public Coin? SelectedCoin
    {
        get => (Coin?)GetValue(SelectedCoinProperty);
        set => SetValue(SelectedCoinProperty, value);
    }

    public string SearchQuery
    {
        get => (string)GetValue(SearchQueryProperty);
        set => SetValue(SearchQueryProperty, value);
    }

    public IEnumerable<Coin>? FilteredCoins
    {
        get => (IEnumerable<Coin>?)GetValue(FilteredCoinsProperty);
        set => SetValue(FilteredCoinsProperty, value);
    }

    private void ApplyFilter()
    {
        var all = AllCoins ?? [];
        var q = SearchQuery?.Trim() ?? string.Empty;

        FilteredCoins = string.IsNullOrEmpty(q)
            ? all
            : all.Where(c =>
                c.Symbol.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
    }
}