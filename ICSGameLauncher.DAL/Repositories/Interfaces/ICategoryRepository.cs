using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<CategoryEntity?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default);
    Task<List<CategoryEntity>> GetCategoriesWithTitlesAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
}