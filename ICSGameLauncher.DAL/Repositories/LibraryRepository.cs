using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class LibraryRepository(ICSGameLauncherDbContext dbContext) : Repository<LibraryEntity>(dbContext), ILibraryRepository
{
    public async Task<LibraryEntity?> GetLibraryWithDetailsAsync(int id, bool trackChanges = true, CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Include(l => l.User)
            .Include(l => l.Titles)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<List<LibraryEntity>> GetLibrariesByUserIdAsync(
        int userId,
        bool hideEmpty = false,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        query = query.Where(l => l.UserId == userId);

        if (hideEmpty)
        {
            query = query.Where(l => l.Titles.Any());
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<LibraryEntity>> GetLibrariesContainingTitleAsync(int titleId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(l => l.Titles.Any(t => t.Id == titleId))
            .ToListAsync(cancellationToken);
    }

}
