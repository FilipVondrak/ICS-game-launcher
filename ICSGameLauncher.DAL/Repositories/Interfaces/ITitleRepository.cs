using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ITitleRepository : IRepository<TitleEntity>
{
    public Task<List<TitleEntity>> GetTitlesByNameAsync(string name, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesByPegiRatingAsync(PegiAge pegi, bool trackChanges = false, CancellationToken ct = default);

    public Task<TitleEntity> GetTitleWithDetailsAsync(Guid id, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesByCategoryAsync(Guid categoryId, bool trackChanges = false, CancellationToken ct = default);

    public Task<List<TitleEntity>> GetTitlesInLibraryAsync(Guid libraryId, bool trackChanges = false, CancellationToken ct = default);

    Task<List<TitleEntity>> GetTitlesByStudioAsync(Guid studioId, bool trackChanges = false, CancellationToken ct = default);
}