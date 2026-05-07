using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class LoginViewModel : ObservableObject
{
    [RelayCommand]
    private static void Login()
    {
        if (Application.Current is not null && Application.Current.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new MainPage(new MainPageViewModel());
        }
    }
}