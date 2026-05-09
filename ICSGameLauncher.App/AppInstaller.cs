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
        services.AddTransient<StoreViewModel>();
        services.AddTransient<StoreView>();
        services.AddTransient<LibrariesViewModel>();
        services.AddTransient<LibrariesView>();
        services.AddTransient<AddGamePopupViewModel>();
        services.AddTransient<AddCategoryStudioPopupViewModel>();

        return services;
    }
}