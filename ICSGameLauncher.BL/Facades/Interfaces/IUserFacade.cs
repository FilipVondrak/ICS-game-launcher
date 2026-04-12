using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.BL.Facades.Interfaces;

public interface IUserFacade
{
    Task<UserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default);

    Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    Task<UserDto> CreateUserAsync(UserDto user, CancellationToken cancellationToken = default);

    Task<UserDto> UpdateUserAsync(UserDto user, CancellationToken cancellationToken = default);

    Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}
