using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class TitleFacade(IUnitOfWorkFactory uowFactory) : ITitleFacade
{
    public async Task<TitleDto> GetTitleAsync(int titleId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        var entity = await repository.GetTitleWithDetailsAsync(titleId, trackChanges: false, cancellationToken);
        return entity.Adapt<TitleDto>();
    }

    public async Task<List<TitleDto>> GetAllTitlesAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        List<TitleEntity> entities = await repository.GetAllAsync(trackChanges: false, cancellationToken);
        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetTitlesByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesByNameAsync(name, ct: cancellationToken);
        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetTitlesByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesByCategoryAsync(categoryId, ct: cancellationToken);
        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetTitlesInLibraryAsync(int libraryId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        List<TitleEntity> entities = await repository.GetTitlesInLibraryAsync(libraryId, ct: cancellationToken);
        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<List<TitleDto>> GetSortedTitlesAsync(
        SortByField sortBy,
        SortDirection direction,
        List<string>? categoryNames = null,
        List<string>? studioNames = null,
        List<PegiAge>? pegiRatings = null,
        bool? ownership = null,
        int? userId = null,
        CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        List<TitleEntity> entities = await repository.GetSortedTitlesAsync(
            sortBy,
            direction,
            categoryNames,
            studioNames,
            pegiRatings,
            ownership,
            userId,
            trackChanges: false,
            ct: cancellationToken);

        return entities.Adapt<List<TitleDto>>();
    }

    public async Task<int> CreateTitleAsync(TitleDto titleDto, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var titleRepository = uow.GetRepository<ITitleRepository>();
        var categoryRepository = uow.GetRepository<ICategoryRepository>();
        var studioRepository = uow.GetRepository<IStudioRepository>();

        TitleEntity entity = titleDto.Adapt<TitleEntity>();

        var trackedStudio = await studioRepository.GetByIdAsync(titleDto.Studios[0].Id, trackChanges: true, cancellationToken);
        entity.Studios.Add(trackedStudio);

        if (titleDto.Categories is not null)
        {
            foreach (var categoryDto in titleDto.Categories)
            {
                var trackedCategory = await categoryRepository.GetByIdAsync(categoryDto.Id, trackChanges: true, cancellationToken);
                entity.Categories.Add(trackedCategory);
            }
        }

        await titleRepository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateTitleAsync(TitleDto titleDto, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();
        var categoryRepository = uow.GetRepository<ICategoryRepository>();
        var studioRepository = uow.GetRepository<IStudioRepository>();

        TitleEntity entity = await repository.GetTitleWithDetailsAsync(titleDto.Id, trackChanges: true, cancellationToken);

        entity.Name = titleDto.Name;
        entity.Description = titleDto.Description;
        entity.PegiRating = titleDto.PegiRating;

        entity.Studios.Clear();
        if (titleDto.Studios is { Count: > 0 })
        {
            var trackedStudio = await studioRepository.GetByIdAsync(titleDto.Studios[0].Id, trackChanges: true, cancellationToken);
            entity.Studios.Add(trackedStudio);
        }

        entity.Categories.Clear();
        if (titleDto.Categories is not null)
        {
            foreach (var categoryDto in titleDto.Categories)
            {
                var trackedCategory = await categoryRepository.GetByIdAsync(categoryDto.Id, trackChanges: true, cancellationToken);
                entity.Categories.Add(trackedCategory);
            }
        }

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }

    public async Task DeleteTitleAsync(int titleId, CancellationToken cancellationToken = default)
    {
        await using var uow = uowFactory.Create();
        var repository = uow.GetRepository<ITitleRepository>();

        await repository.DeleteAsync(titleId, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}
