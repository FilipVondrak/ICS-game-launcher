using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel pageViewModel)
    {
        InitializeComponent();
        BindingContext = pageViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainPageViewModel viewModel)
        {
            viewModel.ViewLoadedCommand.Execute(null);
        }
    }
}