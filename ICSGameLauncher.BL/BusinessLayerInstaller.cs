using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Mappings;

using Mapster;

using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.BL;

public static class BusinessLayerInstaller
{
    public static IServiceCollection RegisterBlServices(this IServiceCollection services)
    {
        services.AddMapster();
        MappingsConfig.Configure();
        TypeAdapterConfig.GlobalSettings.Compile();

        services.AddSingleton<ICategoryFacade, CategoryFacade>();
        services.AddSingleton<ILibraryFacade, LibraryFacade>();
        services.AddSingleton<IStudioFacade, StudioFacade>();
        services.AddSingleton<ITitleFacade, TitleFacade>();
        services.AddSingleton<IUserFacade, UserFacade>();

        return services;
    }
}