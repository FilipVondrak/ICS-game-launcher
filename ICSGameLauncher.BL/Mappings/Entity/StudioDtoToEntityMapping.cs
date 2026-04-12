using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;
using Mapster;

namespace ICSGameLauncher.BL.Mappings.Entity;

public sealed class StudioDtoToEntityMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<StudioDto, StudioEntity>.NewConfig()
            .Ignore(dest => dest.Titles);
    }
}