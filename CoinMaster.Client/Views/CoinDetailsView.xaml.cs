using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CoinMaster.Client.Views;

/// <summary>
/// Логика взаимодействия для CoinDetailsView.xaml
/// </summary>
public partial class CoinDetailsView : UserControl
{
    public CoinDetailsView()
    {
        InitializeComponent();
    }

    private void OpenTradeUrl_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string url && !string.IsNullOrWhiteSpace(url))
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}