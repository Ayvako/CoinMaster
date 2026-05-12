using CoinMaster.Client.Services;
using CoinMaster.Client.ViewModels;
using System.Windows;

namespace CoinMaster.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            _ = viewModel.LoadTopCoinsAsync();
        }
    }
}