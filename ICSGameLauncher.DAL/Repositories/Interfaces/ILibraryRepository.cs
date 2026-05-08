using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface ILibraryRepository : IRepository<LibraryEntity>
{
    public Task<LibraryEntity?> GetLibraryWithDetailsAsync(Guid id, bool trackChanges = true, CancellationToken cancellationToken = default);

    public Task<List<LibraryEntity>> GetLibrariesByUserIdAsync(Guid userId, bool trackChanges = false, CancellationToken cancellationToken = default);

    public Task<List<LibraryEntity>> GetLibrariesContainingTitleAsync(Guid titleId, bool trackChanges = false, CancellationToken cancellationToken = default);

}