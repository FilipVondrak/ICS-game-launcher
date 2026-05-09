using CommunityToolkit.Mvvm.Messaging;

using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<StoreViewModel>();
        services.AddTransient<StoreView>();
        services.AddTransient<LibrariesViewModel>();
        services.AddTransient<LibrariesView>();
        services.AddTransient<AddGamePopupViewModel>();
        services.AddTransient<GameDetailsViewModel>();
        services.AddTransient<GameDetailsView>();
        services.AddTransient<LoginPage>();
        services.AddTransient<MainPage>();
        services.AddTransient<LibraryDetailView>();
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<LibraryDetailViewModel>();
        services.AddTransient<AddCategoryStudioPopupViewModel>();

        return services;
    }
}