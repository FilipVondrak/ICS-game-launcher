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

    public async Task<List<LibraryEntity>> GetLibrariesByUserIdAsync(int userId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(l => l.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LibraryEntity>> GetLibrariesContainingTitleAsync(int titleId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(l => l.Titles.Any(t => t.Id == titleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LibraryEntity>> GetSortedLibrariesByUserIdAsync(
        int userId,
        bool sortAlphabetAsc,
        bool sortAlphabetDesc,
        bool sortTitlesAsc,
        bool sortTitlesDesc,
        bool hideEmpty = false,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<LibraryEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        query = query
            .Where(l => l.UserId == userId)
            .Include(l => l.Titles);

        if (hideEmpty)
        {
            query = query.Where(l => l.Titles.Count > 0);
        }

        bool hasAlphabetSort = sortAlphabetAsc || sortAlphabetDesc;
        bool hasTitleSort = sortTitlesAsc || sortTitlesDesc;

        if (hasAlphabetSort)
        {
            IOrderedQueryable<LibraryEntity> ordered = sortAlphabetDesc
                ? query.OrderByDescending(l => l.Description)
                : query.OrderBy(l => l.Description);

            if (sortTitlesAsc)
            {
                ordered = ordered.ThenBy(l => l.Titles.Count);
            }
            else if (sortTitlesDesc)
            {
                ordered = ordered.ThenByDescending(l => l.Titles.Count);
            }

            return await ordered.ToListAsync(cancellationToken);
        }

        if (hasTitleSort)
        {
            query = sortTitlesDesc
                ? query.OrderByDescending(l => l.Titles.Count)
                : query.OrderBy(l => l.Titles.Count);
        }

        return await query.ToListAsync(cancellationToken);
    }

}