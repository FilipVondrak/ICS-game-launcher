using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;
using Mapster;

namespace ICSGameLauncher.BL.Facades;

public sealed class CategoryFacade(IUnitOfWorkFactory unitOfWorkFactory) : ICategoryFacade
{
    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();
        var entities = await repository.GetAllAsync(false, cancellationToken);
        return entities.Adapt<List<CategoryDto>>();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();
        var entity = await repository.GetByIdAsync(id, false, cancellationToken);
        return entity?.Adapt<CategoryDto>();
    }

    public async Task<CategoryDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();
        var entity = await repository.GetByNameAsync(name, false, cancellationToken);
        return entity?.Adapt<CategoryDto>();
    }

    public async Task<List<CategoryDto>> GetCategoriesWithTitlesAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();
        var entities = await repository.GetCategoriesWithTitlesAsync(false, cancellationToken);
        return entities.Adapt<List<CategoryDto>>();
    }

    public async Task<CategoryDto> InsertAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();
        var entity = categoryDto.Adapt<CategoryEntity>();

        await repository.InsertAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);

        return entity.Adapt<CategoryDto>();
    }

    public async Task UpdateAsync(CategoryDto categoryDto, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();

        var entity = await repository.GetByIdAsync(categoryDto.Id, true, cancellationToken);
        categoryDto.Adapt(entity);

        await repository.UpdateAsync(entity, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ICategoryRepository>();

        await repository.DeleteAsync(id, cancellationToken);
        await uow.CommitAsync(cancellationToken);
    }
}