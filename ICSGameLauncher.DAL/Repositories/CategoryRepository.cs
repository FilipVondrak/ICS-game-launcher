using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class CategoryRepository (ICSGameLauncherDbContext dbContext) : Repository<CategoryEntity>(dbContext), ICategoryRepository
{
    public async Task<CategoryEntity?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        IQueryable<CategoryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }

    public async Task<List<CategoryEntity>> GetCategoriesWithTitlesAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<CategoryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query.Include(c => c.Titles).ToListAsync(cancellationToken);
    }
}