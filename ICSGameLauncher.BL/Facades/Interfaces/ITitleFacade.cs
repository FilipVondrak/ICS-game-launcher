using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.BL.Facades.Interfaces;

public interface ITitleFacade
{
    Task<TitleDto> GetTitleAsync(int titleId, CancellationToken cancellationToken = default);

    Task<List<TitleDto>> GetAllTitlesAsync(CancellationToken cancellationToken = default);

    Task<List<TitleDto>> GetTitlesByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<List<TitleDto>> GetTitlesByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<List<TitleDto>> GetTitlesInLibraryAsync(int libraryId, CancellationToken cancellationToken = default);

    Task<List<TitleDto>> GetSortedTitlesAsync(
        SortByField sortBy,
        SortDirection direction,
        List<string>? categoryNames = null,
        List<string>? studioNames = null,
        List<PegiAge>? pegiRatings = null,
        bool? ownership = null,
        int? userId = null,
        CancellationToken cancellationToken = default);

    Task<int> CreateTitleAsync(TitleDto titleDto, CancellationToken cancellationToken = default);

    Task DeleteTitleAsync(int titleId, CancellationToken cancellationToken = default);
}
