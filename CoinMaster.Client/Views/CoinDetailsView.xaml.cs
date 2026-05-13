namespace CoinMaster.Client.Views;

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Логика взаимодействия для CoinDetailsView.xaml.
/// </summary>
public partial class CoinDetailsView : UserControl
{
    public CoinDetailsView()
    {
        this.InitializeComponent();
    }

    private void OpenTradeUrl_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string url && !string.IsNullOrWhiteSpace(url))
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}