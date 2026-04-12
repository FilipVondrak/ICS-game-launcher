using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class TitleFacade(IUnitOfWorkFactory uowFactory) : ITitleFacade
{
    public async Task<TitleDto> GetTitleAsync(int titleId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        TitleEntity entity = await repository.GetByIdAsync(titleId, trackChanges: false, cancellationToken);

        return entity.Adapt<TitleDto>();
    }

    public async Task<List<TitleDto>> GetAllTitlesAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        List<TitleEntity> entities = await repository.GetAllAsync(trackChanges: false, cancellationToken);

        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetTitlesByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesByNameAsync(name, cancellationToken);

        return entities.Adapt<List<TitleDto>>();
    }


    public async Task<List<TitleDto>> GetTitlesByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesByCategoryAsync(categoryId, cancellationToken);

        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetTitlesInLibraryAsync(int libraryId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesInLibraryAsync(libraryId, cancellationToken);

        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<int> CreateTitleAsync(TitleDto titleDto, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        TitleEntity entity = titleDto.Adapt<TitleEntity>();

        await repository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return entity.Id;
    }

    public async Task DeleteTitleAsync(int titleId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<TitleRepository>();

        await repository.DeleteAsync(titleId, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}