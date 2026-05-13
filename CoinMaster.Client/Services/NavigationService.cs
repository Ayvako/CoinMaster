namespace CoinMaster.Client.Services;

using CoinMaster.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

public class NavigationService(IServiceProvider sp)
    : INavigationService
{
    private readonly Stack<object> history = new();

    public event EventHandler<bool>? CanGoBackChanged;

    public bool CanGoBack => this.history.Count > 0;

    private MainWindowViewModel MainVM => sp.GetRequiredService<MainWindowViewModel>();

    public void NavigateTo<TViewModel>()
        where TViewModel : class
        => this.SetViewModel(sp.GetRequiredService<TViewModel>());

    public void NavigateTo<TViewModel>(Action<TViewModel> configure)
        where TViewModel : class
    {
        var vm = sp.GetRequiredService<TViewModel>();
        configure(vm);
        this.SetViewModel(vm);
    }

    public void GoBack()
    {
        if (this.history.TryPop(out var prev))
        {
            this.MainVM.CurrentViewModel = prev;
        }

        this.CanGoBackChanged?.Invoke(this, this.CanGoBack);
    }

    public async Task NavigateToAsync<TViewModel>(Func<TViewModel, Task> configure)
    where TViewModel : class
    {
        var vm = sp.GetRequiredService<TViewModel>();
        this.SetViewModel(vm);
        await configure(vm);
    }

    private void SetViewModel(object vm)
    {
        if (this.MainVM.CurrentViewModel == vm)
        {
            return;
        }

        if (this.MainVM.CurrentViewModel is not null)
        {
            this.history.Push(this.MainVM.CurrentViewModel);
        }

        this.MainVM.CurrentViewModel = vm;
        this.CanGoBackChanged?.Invoke(this, this.CanGoBack);
    }
}