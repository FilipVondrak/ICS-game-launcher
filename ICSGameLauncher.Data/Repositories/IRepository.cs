using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICSGameLauncher.Data.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}