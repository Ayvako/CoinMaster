namespace CoinMaster.Client.Views;

using System.Windows;
using System.Windows.Controls;
using CoinMaster.Core.Entities;

public partial class CoinSelectorControl : UserControl
{
    public static readonly DependencyProperty AllCoinsProperty =
        DependencyProperty.Register(
            nameof(AllCoins),
            typeof(IEnumerable<Coin>),
            typeof(CoinSelectorControl),
            new PropertyMetadata(null, (d, _) => ((CoinSelectorControl)d).ApplyFilter()));

    public static readonly DependencyProperty SelectedCoinProperty =
        DependencyProperty.Register(
            nameof(SelectedCoin),
            typeof(Coin),
            typeof(CoinSelectorControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SearchQueryProperty =
        DependencyProperty.Register(
            nameof(SearchQuery),
            typeof(string),
            typeof(CoinSelectorControl),
            new PropertyMetadata(string.Empty, (d, _) => ((CoinSelectorControl)d).ApplyFilter()));

    public static readonly DependencyProperty FilteredCoinsProperty =
        DependencyProperty.Register(
            nameof(FilteredCoins),
            typeof(IEnumerable<Coin>),
            typeof(CoinSelectorControl));

    public CoinSelectorControl() => this.InitializeComponent();

    public IEnumerable<Coin>? AllCoins
    {
        get => (IEnumerable<Coin>?)this.GetValue(AllCoinsProperty);
        set => this.SetValue(AllCoinsProperty, value);
    }

    public Coin? SelectedCoin
    {
        get => (Coin?)this.GetValue(SelectedCoinProperty);
        set => this.SetValue(SelectedCoinProperty, value);
    }

    public string SearchQuery
    {
        get => (string)this.GetValue(SearchQueryProperty);
        set => this.SetValue(SearchQueryProperty, value);
    }

    public IEnumerable<Coin>? FilteredCoins
    {
        get => (IEnumerable<Coin>?)this.GetValue(FilteredCoinsProperty);
        set => this.SetValue(FilteredCoinsProperty, value);
    }

    private void ApplyFilter()
    {
        var all = this.AllCoins ?? [];
        var q = this.SearchQuery?.Trim() ?? string.Empty;

        this.FilteredCoins = string.IsNullOrEmpty(q)
            ? all
            : all.Where(c =>
                c.Symbol.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
    }
}