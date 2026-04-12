using System.Globalization;

using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class TitleRepository(ICSGameLauncherDbContext dbContext) : Repository<TitleEntity>(dbContext), ITitleRepository
{
    public async Task<List<TitleEntity>> GetTitlesByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => EF.Functions.Like(t.Name, $"%{name}%"))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TitleEntity>> GetTitlesByPegiRatingAsync(PegiAge pegi, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => t.PegiRating == pegi)
            .ToListAsync(cancellationToken);
    }

    public async Task<TitleEntity?> GetTitleWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.Studios)
            .Include(t => t.Categories)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<TitleEntity>> GetTitlesByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => t.Categories.Any(c => c.Id == categoryId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TitleEntity>> GetTitlesInLibraryAsync(int libraryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => t.Libraries.Any(l => l.Id == libraryId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TitleEntity>> GetTitlesByStudioAsync(int studioId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => t.Studios.Any(s => s.Id == studioId))
            .ToListAsync(cancellationToken);
    }
}