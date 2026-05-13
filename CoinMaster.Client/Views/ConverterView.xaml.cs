using CoinMaster.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CoinMaster.Client.Views;

/// <summary>
/// Логика взаимодействия для ConverterView.xaml
/// </summary>
public partial class ConverterView : UserControl
{
    public ConverterView()
    {
        InitializeComponent();
        Loaded += ConverterView_Loaded;
    }

    private async void ConverterView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ConverterViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}