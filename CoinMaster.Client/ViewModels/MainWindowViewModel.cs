using CommunityToolkit.Mvvm.ComponentModel;

namespace CoinMaster.Client.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private object currentViewModel;

    public MainWindowViewModel(MainViewModel mainVm)
    {
        currentViewModel = mainVm;
    }
}