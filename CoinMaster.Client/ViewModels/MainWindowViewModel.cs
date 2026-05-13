using CoinMaster.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoinMaster.Client.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public List<string> AvailableLanguages { get; } = new() { "ua", "en" };

    [ObservableProperty]
    private object currentViewModel;

    [ObservableProperty]
    private bool canNavigateBack;

    private string selectedLanguage = "ua";

    private readonly INavigationService navigation;

    [ObservableProperty]
    private string themeIcon = "☀";

    [ObservableProperty]
    private bool isOnConverter;

    partial void OnCurrentViewModelChanged(object value)
        => IsOnConverter = value is ConverterViewModel;

    private void UpdateThemeIcon() => ThemeIcon = ThemeManager.IsDark ? "☀" : "☾";

    public MainWindowViewModel(MainViewModel mainVm, INavigationService navigation)
    {
        this.navigation = navigation;
        currentViewModel = mainVm;
        ThemeManager.Initialize();
        UpdateThemeIcon();
        ThemeManager.ThemeChanged += (s, isDark) => UpdateThemeIcon();

        navigation.CanGoBackChanged += (_, value) =>
        {
            CanNavigateBack = value;
            GoBackCommand.NotifyCanExecuteChanged();
        };
    }

    public string SelectedLanguage
    {
        get => selectedLanguage;

        set
        {
            if (selectedLanguage != value)
            {
                selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                LocalizationService.SetLanguage(value);
            }
        }
    }

    [RelayCommand]
    private void NavigateToConverter()
    {
        navigation.NavigateTo<ConverterViewModel>();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeManager.Toggle();
        UpdateThemeIcon();
    }

    private bool CanGoBack() => navigation.CanGoBack;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void GoBack()
    {
        navigation.GoBack();
        CanNavigateBack = navigation.CanGoBack;
    }
}