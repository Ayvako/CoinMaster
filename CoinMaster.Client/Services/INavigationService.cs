namespace CoinMaster.Client.Services;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : class;

    void NavigateTo<TViewModel>(Action<TViewModel> configure) where TViewModel : class;

    Task NavigateToAsync<TViewModel>(Func<TViewModel, Task> configure) where TViewModel : class;

    void GoBack();

    event EventHandler<bool> CanGoBackChanged;

    bool CanGoBack { get; }
}