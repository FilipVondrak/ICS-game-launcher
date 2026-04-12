using ICSGameLauncher.BL;
using ICSGameLauncher.DAL;
using Microsoft.Extensions.Logging;

namespace ICSGameLauncher.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
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

        return builder.Build();
    }
}