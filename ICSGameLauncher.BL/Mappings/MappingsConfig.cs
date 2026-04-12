using ICSGameLauncher.BL.Mappings.Dto;
using ICSGameLauncher.BL.Mappings.Entity;

namespace ICSGameLauncher.BL.Mappings;

public static class MappingsConfig
{
    public static void Configure()
    {
        TitleEntityToDtoMapping.Configure();
        TitleDtoToEntityMapping.Configure();
    }
}