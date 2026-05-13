namespace CoinMaster.Client.ViewModels;

using CoinMaster.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService navigation;

    [ObservableProperty]
    private object currentViewModel;

    [ObservableProperty]
    private bool canNavigateBack;

    [ObservableProperty]
    private string selectedLanguage = "ua";

    [ObservableProperty]
    private string themeIcon = "☀";

    [ObservableProperty]
    private bool isOnConverter;

    public MainWindowViewModel(MainViewModel mainVm, INavigationService navigation)
    {
        this.navigation = navigation;
        this.currentViewModel = mainVm;
        ThemeManager.Initialize();
        this.UpdateThemeIcon();
        ThemeManager.ThemeChanged += (s, isDark) => this.UpdateThemeIcon();

        navigation.CanGoBackChanged += (_, value) =>
        {
            this.CanNavigateBack = value;
            this.GoBackCommand.NotifyCanExecuteChanged();
        };
    }

    public List<string> AvailableLanguages { get; } = ["ua", "en"];

    private void UpdateThemeIcon() => this.ThemeIcon = ThemeManager.IsDark ? "☀" : "☾";

    partial void OnSelectedLanguageChanged(string value)
    {
        LocalizationService.SetLanguage(value);
    }

    partial void OnCurrentViewModelChanged(object value)
        => IsOnConverter = value is ConverterViewModel;

    [RelayCommand]
    private void NavigateToConverter()
    {
        this.navigation.NavigateTo<ConverterViewModel>();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeManager.Toggle();
        this.UpdateThemeIcon();
    }

    private bool CanGoBack() => this.navigation.CanGoBack;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void GoBack()
    {
        this.navigation.GoBack();
        this.CanNavigateBack = this.navigation.CanGoBack;
    }
}