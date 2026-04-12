using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class StudioRepository(ICSGameLauncherDbContext dbContext) : Repository<StudioEntity>(dbContext), IStudioRepository
{
    public async Task<StudioEntity?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        IQueryable<StudioEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }

    public async Task<List<StudioEntity>> GetStudiosWithTitlesAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<StudioEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query.Include(s => s.Titles).ToListAsync(cancellationToken);
    }
}