using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;
using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class UserFacade(IUnitOfWorkFactory uowFactory) : IUserFacade
{
    public async Task<UserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<IUserRepository>();

        UserEntity entity = await repository.GetByIdAsync(userId, trackChanges: false, cancellationToken);
        return entity.Adapt<UserDto>();
    }

    public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<IUserRepository>();

        List<UserEntity> entities = await repository.GetAllAsync(trackChanges: false, cancellationToken);
        return entities.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> CreateUserAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<IUserRepository>();

        UserEntity entity = user.Adapt<UserEntity>();
        await repository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);

        return entity.Adapt<UserDto>();
    }

    public async Task<UserDto> UpdateUserAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<IUserRepository>();

        UserEntity entity = user.Adapt<UserEntity>();
        await repository.UpdateAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);

        return entity.Adapt<UserDto>();
    }

    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<IUserRepository>();

        await repository.DeleteAsync(userId, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}
