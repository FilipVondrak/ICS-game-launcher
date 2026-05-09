using System.Globalization;

using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class TitleRepository(ICSGameLauncherDbContext dbContext) : Repository<TitleEntity>(dbContext), ITitleRepository
{
    public async Task<List<TitleEntity>> GetTitlesByNameAsync(
        string name,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        string searchTerm = name.ToLowerInvariant();
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();

        // disable warnings because of sql conversion
#pragma warning disable CA1862, CA1311, CA1304
        return await query
            .Where(t => t.Name.ToLower().Contains(searchTerm))
            .ToListAsync(ct);
#pragma warning restore CA1862, CA1311, CA1304
    }

    public async Task<List<TitleEntity>> GetTitlesByPegiRatingAsync(
        PegiAge pegi,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(t => t.PegiRating == pegi)
            .ToListAsync(ct);
    }

    public async Task<TitleEntity> GetTitleWithDetailsAsync(
        int id,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
                   .Include(t => t.Studios)
                   .Include(t => t.Categories)
                   .FirstOrDefaultAsync(t => t.Id == id, ct)
               ?? throw new EntityNotFoundException(entityName: nameof(TitleEntity), id);
    }

    public async Task<List<TitleEntity>> GetTitlesByCategoryAsync(
        int categoryId,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(t => t.Categories.Any(c => c.Id == categoryId))
            .ToListAsync(ct);
    }

    public async Task<List<TitleEntity>> GetTitlesInLibraryAsync(
        int libraryId,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(t => t.Libraries.Any(l => l.Id == libraryId))
            .ToListAsync(ct);
    }

    public async Task<List<TitleEntity>> GetTitlesByStudioAsync(
        int studioId,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query
            .Where(t => t.Studios.Any(s => s.Id == studioId))
            .ToListAsync(ct);
    }

    public async Task<List<TitleEntity>> GetFilteredTitlesAsync(
        List<string>? categoryNames,
        List<string>? studioNames,
        List<PegiAge>? pegiRatings,
        bool? ownership,
        int? userId,
        int? libraryId,
        bool trackChanges = false,
        CancellationToken ct = default)
    {
        IQueryable<TitleEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        query = query
            .Include(t => t.Categories)
            .Include(t => t.Studios)
            .Include(t => t.Libraries);

        if (libraryId is int selectedLibraryId)
        {
            query = query.Where(t => t.Libraries.Any(l => l.Id == selectedLibraryId));
        }

        if (categoryNames is { Count: > 0 })
        {
            query = query.Where(t => t.Categories.Any(c => categoryNames.Contains(c.Name)));
        }

        if (studioNames is { Count: > 0 })
        {
            query = query.Where(t => t.Studios.Any(s => studioNames.Contains(s.Name)));
        }

        if (pegiRatings is { Count: > 0 })
        {
            query = query.Where(t => pegiRatings.Contains(t.PegiRating));
        }

        if (ownership.HasValue && userId.HasValue)
        {
            if (ownership.Value)
            {
                query = query.Where(t => t.Libraries.Any(l => l.UserId == userId.Value));
            }
            else
            {
                query = query.Where(t => !t.Libraries.Any(l => l.UserId == userId.Value));
            }
        }

        return await query.ToListAsync(ct);
    }
}
