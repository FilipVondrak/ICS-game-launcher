using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel pageViewModel)
    {
        InitializeComponent();
        BindingContext = pageViewModel;

        pageViewModel.PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(MainPageViewModel.IsProfileMenuVisible))
            {
                if (pageViewModel.IsProfileMenuVisible)
                    await OpenMenu();
                else
                    await CloseMenu();
            }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainPageViewModel viewModel)
        {
            viewModel.ViewLoadedCommand.Execute(null);
        }
    }

    private async Task OpenMenu()
    {
        DropdownMenu.IsVisible = true;

        await Task.WhenAll(
            DropdownMenu.FadeToAsync(1, 250, Easing.CubicOut),
            DropdownMenu.TranslateToAsync(0, 0, 250, Easing.CubicOut)
        );
    }

    private async Task CloseMenu()
    {
        await Task.WhenAll(
            DropdownMenu.FadeToAsync(0, 200, Easing.CubicIn),
            DropdownMenu.TranslateToAsync(0, -20, 200, Easing.CubicIn)
        );
        DropdownMenu.IsVisible = false;
    }
}