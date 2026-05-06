using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ITitleRepository : IRepository<TitleEntity>
{
    public Task<List<TitleEntity>> GetTitlesByNameAsync(string name, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesByPegiRatingAsync(PegiAge pegi, bool trackChanges = false, CancellationToken ct = default);

    public Task<TitleEntity> GetTitleWithDetailsAsync(int id, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesByCategoryAsync(int categoryId, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesInLibraryAsync(int libraryId, bool trackChanges = false, CancellationToken ct = default);

    Task<List<TitleEntity>> GetTitlesByStudioAsync(int studioId, bool trackChanges = false, CancellationToken ct = default);
}