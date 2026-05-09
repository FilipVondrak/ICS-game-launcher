using CommunityToolkit.Maui;
using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL;
using ICSGameLauncher.BL.Mappings;
using ICSGameLauncher.DAL;
using Microsoft.Extensions.Logging;

namespace ICSGameLauncher.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MappingsConfig.Configure();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        string dataDirectory = Path.Combine(FileSystem.AppDataDirectory);

        builder.Services.RegisterDalServices(dataDirectory);
        builder.Services.RegisterBlServices();
        builder.Services.AddAppServices();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ICSGameLauncherDbContext>();

            dbContext.Database.EnsureCreated();

            DatabaseSeeder.Seed(dbContext);
        }

        return app;
    }
}