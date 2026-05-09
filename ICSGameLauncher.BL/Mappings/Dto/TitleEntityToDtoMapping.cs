using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;

using Mapster;

namespace ICSGameLauncher.BL.Mappings.Dto;

public sealed class TitleEntityToDtoMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<TitleEntity, TitleDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PegiRating, src => src.PegiRating)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Studios, src => src.Studios)
            .Map(dest => dest.Categories, src => src.Categories);
    }
}