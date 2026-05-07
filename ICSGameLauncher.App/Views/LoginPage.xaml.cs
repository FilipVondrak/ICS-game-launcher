using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}