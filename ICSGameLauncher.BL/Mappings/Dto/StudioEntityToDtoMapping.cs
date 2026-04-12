using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;
using Mapster;

namespace ICSGameLauncher.BL.Mappings.Dto;

public sealed class StudioEntityToDtoMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<StudioEntity, StudioDto>.NewConfig();
    }
}