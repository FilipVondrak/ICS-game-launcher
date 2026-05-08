using System.Collections.Generic;
using System.Threading.Tasks;
using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories.Interfaces;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<List<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default);
}