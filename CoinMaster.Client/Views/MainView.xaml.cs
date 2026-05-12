using CoinMaster.Client.ViewModels;
using System.Windows.Controls;

namespace CoinMaster.Client.Views;

/// <summary>
/// Логика взаимодействия для MainView.xaml
/// </summary>
public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        Loaded += MainView_Loaded;
    }

    private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            _ = viewModel.LoadTopCoinsAsync();
        }
    }
}