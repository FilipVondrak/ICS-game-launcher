using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;
using Mapster;

namespace ICSGameLauncher.BL.Mappings.Entity;

public sealed class CategoryDtoToEntityMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<CategoryDto, CategoryEntity>.NewConfig()
            .Ignore(dest => dest.Titles);
    }
}