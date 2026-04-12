using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;
using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class StudioFacade(IUnitOfWorkFactory unitOfWorkFactory) : IStudioFacade
{
    public async Task<List<StudioDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();
        var entities = await repository.GetAllAsync(false, cancellationToken);
        return entities.Adapt<List<StudioDto>>();
    }

    public async Task<StudioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();
        var entity = await repository.GetByIdAsync(id, false, cancellationToken);
        return entity?.Adapt<StudioDto>();
    }

    public async Task<StudioDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();
        var entity = await repository.GetByNameAsync(name, false, cancellationToken);
        return entity?.Adapt<StudioDto>();
    }

    public async Task<List<StudioDto>> GetStudiosWithTitlesAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();
        var entities = await repository.GetStudiosWithTitlesAsync(false, cancellationToken);
        return entities.Adapt<List<StudioDto>>();
    }

    public async Task<StudioDto> InsertAsync(StudioDto studioDto, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();
        var entity = studioDto.Adapt<StudioEntity>();

        await repository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);

        return entity.Adapt<StudioDto>();
    }

    public async Task UpdateAsync(StudioDto studioDto, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();

        var entity = await repository.GetByIdAsync(studioDto.Id, true, cancellationToken);
        studioDto.Adapt(entity);

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<IStudioRepository>();

        await repository.DeleteAsync(id, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}