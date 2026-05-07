using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App;

public sealed partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel pageViewModel)
    {
        InitializeComponent();
        BindingContext = pageViewModel;
    }
}