using CommunityToolkit.Mvvm.Messaging;

using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LoginPage>();

        services.AddTransient<MainPageViewModel>();
        services.AddTransient<MainPage>();

        return services;
    }
}