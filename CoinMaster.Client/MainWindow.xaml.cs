namespace CoinMaster.Client;

using System.Windows;
using CoinMaster.Client.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Loaded += this.MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            await viewModel.LoadTopCoinsAsync();
        }
    }
}