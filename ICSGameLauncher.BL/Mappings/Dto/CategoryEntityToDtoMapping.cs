using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;
using Mapster;

namespace ICSGameLauncher.BL.Mappings.Dto;

public sealed class CategoryEntityToDtoMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<CategoryEntity, CategoryDto>.NewConfig();
    }
}