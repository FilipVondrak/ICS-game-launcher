using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ITitleRepository : IRepository<TitleEntity>
{
    public Task<List<TitleEntity>> GetTitlesByNameAsync(string name, CancellationToken cancellationToken = default);

    public Task<List<TitleEntity>> GetTitlesByPegiRatingAsync(PegiAge pegi, CancellationToken cancellationToken = default);

    public Task<TitleEntity?> GetTitleWithDetailsAsync(int id, CancellationToken cancellationToken = default);

    public Task<List<TitleEntity>> GetTitlesByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    public Task<List<TitleEntity>> GetTitlesInLibraryAsync(int libraryId, CancellationToken cancellationToken = default);

    Task<List<TitleEntity>> GetTitlesByStudioAsync(int studioId, CancellationToken cancellationToken = default);
}