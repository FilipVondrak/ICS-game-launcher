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

        return services;
    }
}