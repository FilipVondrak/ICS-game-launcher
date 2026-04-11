using System.Collections.Generic;
using System.Threading.Tasks;
using ICSGameLauncher.Data.Models;

namespace ICSGameLauncher.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<List<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(int id, bool trackChanges = true, CancellationToken cancellationToken = default);
}