using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();
        MainPage = loginPage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(MainPage);
    }
}