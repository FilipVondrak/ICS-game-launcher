using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class LibraryFacade(IUnitOfWorkFactory uowFactory) : ILibraryFacade
{
    public async Task<LibraryDto?> GetLibraryAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        var entity = await repository.GetLibraryWithDetailsAsync(id, trackChanges: false, cancellationToken);

        return entity?.Adapt<LibraryDto>();
    }

    public async Task<List<LibraryDto>> GetAllLibrariesAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        var entities = await repository.GetAllAsync(trackChanges: false, cancellationToken);

        return entities.Adapt<List<LibraryDto>>();
    }

    public async Task<List<LibraryDto>> GetLibrariesByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        var entities = await repository.GetLibrariesByUserIdAsync(userId, trackChanges: false, cancellationToken);

        return entities.Adapt<List<LibraryDto>>();
    }

    public async Task<int> CreateLibraryAsync(LibraryDto libraryDto, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        var entity = libraryDto.Adapt<LibraryEntity>();

        await repository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateLibraryAsync(LibraryDto libraryDto, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        var entity = libraryDto.Adapt<LibraryEntity>();

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }

    public async Task DeleteLibraryAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ILibraryRepository>();

        await repository.DeleteAsync(id, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}