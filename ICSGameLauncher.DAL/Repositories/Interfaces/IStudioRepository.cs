using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface IStudioRepository : IRepository<StudioEntity>
{
    Task<StudioEntity?> GetByNameAsync(string name, bool trackChanges = true, CancellationToken cancellationToken = default);
    Task<List<StudioEntity>> GetStudiosWithTitlesAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
}