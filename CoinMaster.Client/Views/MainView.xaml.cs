namespace CoinMaster.Client.Views;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoinMaster.Client.ViewModels;
using CoinMaster.Core.Entities;

/// <summary>
/// Логика взаимодействия для MainView.xaml.
/// </summary>
public partial class MainView : UserControl
{
    public MainView()
    {
        this.InitializeComponent();
        this.Loaded += this.MainView_Loaded;
    }

    private async void MainView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    private async void SelectCoin(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row &&
            row.DataContext is Coin coin &&
            this.DataContext is MainViewModel viewModel)
        {
            await viewModel.SelectCoinAsync(coin);
        }
    }
}