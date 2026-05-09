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

    Task<List<TitleDto>> GetFilteredTitlesAsync(
        List<string>? categoryNames,
        List<string>? studioNames,
        List<PegiAge>? pegiRatings,
        bool? ownership,
        int? userId,
        int? libraryId,
        CancellationToken cancellationToken = default);

    Task<int> CreateTitleAsync(TitleDto titleDto, CancellationToken cancellationToken = default);

    Task DeleteTitleAsync(int titleId, CancellationToken cancellationToken = default);
}
