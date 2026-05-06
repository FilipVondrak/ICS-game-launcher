using System.Runtime.CompilerServices;
using ICSGameLauncher.BL.Mappings;
using Mapster;

namespace ICSGameLauncher.BL.Tests;

public static class Setup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        MappingsConfig.Configure();
        TypeAdapterConfig.GlobalSettings.Compile();
    }
}