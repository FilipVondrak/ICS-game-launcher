using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.DAL.Models;
using Mapster;

namespace ICSGameLauncher.BL.Mappings.Dto;

public sealed class UserEntityToDtoMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<UserEntity, UserDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Surname, src => src.Surname)
            .Map(dest => dest.Email, src => src.Email);
    }
}
