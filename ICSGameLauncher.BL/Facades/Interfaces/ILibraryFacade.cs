using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.BL.Facades.Interfaces;

public interface ILibraryFacade
{
    Task<LibraryDto?> GetLibraryAsync(int id, CancellationToken cancellationToken = default);

    Task<List<LibraryDto>> GetAllLibrariesAsync(CancellationToken cancellationToken = default);

    Task<List<LibraryDto>> GetSortedLibrariesByUserIdAsync(
        int userId,
        bool sortAlphabetAsc,
        bool sortAlphabetDesc,
        bool sortTitlesAsc,
        bool sortTitlesDesc,
        bool hideEmpty = false,
        CancellationToken cancellationToken = default);

    Task<int> CreateLibraryAsync(LibraryDto libraryDto, CancellationToken cancellationToken = default);

    Task UpdateLibraryAsync(LibraryDto libraryDto, CancellationToken cancellationToken = default);

    Task DeleteLibraryAsync(int id, CancellationToken cancellationToken = default);

    Task RemoveTitleFromLibraryAsync(int libraryId, int titleId,
        CancellationToken cancellationToken = default);
}