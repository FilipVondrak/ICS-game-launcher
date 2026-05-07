using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new LoginPage(new LoginViewModel()));
    }
}