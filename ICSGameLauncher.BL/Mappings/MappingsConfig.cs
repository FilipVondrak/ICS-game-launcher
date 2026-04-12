using ICSGameLauncher.BL.Mappings.Dto;
using ICSGameLauncher.BL.Mappings.Entity;

namespace ICSGameLauncher.BL.Mappings;

public static class MappingsConfig
{
    public static void Configure()
    {
        CategoryEntityToDtoMapping.Configure();
        StudioEntityToDtoMapping.Configure();
        CategoryDtoToEntityMapping.Configure();
        StudioDtoToEntityMapping.Configure();
        TitleEntityToDtoMapping.Configure();
        TitleDtoToEntityMapping.Configure();
        UserEntityToDtoMapping.Configure();
        UserDtoToEntityMapping.Configure();
    }
}
