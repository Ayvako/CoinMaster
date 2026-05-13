namespace CoinMaster.Client.Views;

using System.Windows;
using System.Windows.Controls;
using CoinMaster.Client.ViewModels;

/// <summary>
/// Логика взаимодействия для ConverterView.xaml.
/// </summary>
public partial class ConverterView : UserControl
{
    public ConverterView()
    {
        this.InitializeComponent();
        this.Loaded += this.ConverterView_Loaded;
    }

    private async void ConverterView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ConverterViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}