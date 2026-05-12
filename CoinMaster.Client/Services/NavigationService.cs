using CoinMaster.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoinMaster.Client.Services;

public class NavigationService(IServiceProvider sp)
    : INavigationService
{
    private readonly Stack<object> history = new();

    private MainWindowViewModel MainVM => sp.GetRequiredService<MainWindowViewModel>();

    public bool CanGoBack => history.Count > 0;

    public event EventHandler<bool>? CanGoBackChanged;

    public void NavigateTo<TViewModel>() where TViewModel : class
        => SetViewModel(sp.GetRequiredService<TViewModel>());

    public void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : class
    {
        var vm = sp.GetRequiredService<TViewModel>();
        configure(vm);
        SetViewModel(vm);
    }

    public void GoBack()
    {
        if (history.TryPop(out var prev))
            MainVM.CurrentViewModel = prev;

        CanGoBackChanged?.Invoke(this, CanGoBack);
    }

    private void SetViewModel(object vm)
    {
        if (MainVM.CurrentViewModel == vm) return;

        if (MainVM.CurrentViewModel is not null)
            history.Push(MainVM.CurrentViewModel);

        MainVM.CurrentViewModel = vm;
        CanGoBackChanged?.Invoke(this, CanGoBack);
    }

    public async Task NavigateToAsync<TViewModel>(Func<TViewModel, Task> configure) where TViewModel : class
    {
        var vm = sp.GetRequiredService<TViewModel>();
        SetViewModel(vm);
        await configure(vm);
    }
}