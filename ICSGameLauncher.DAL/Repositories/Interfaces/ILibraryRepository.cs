using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ILibraryRepository : IRepository<LibraryEntity>
{
    public Task<LibraryEntity?> GetLibraryWithDetailsAsync(int id, bool trackChanges = true, CancellationToken cancellationToken = default);

    public Task<List<LibraryEntity>> GetLibrariesByUserIdAsync(int userId, bool trackChanges = false, CancellationToken cancellationToken = default);

    public Task<List<LibraryEntity>> GetLibrariesContainingTitleAsync(int titleId, bool trackChanges = false, CancellationToken cancellationToken = default);

}