using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL.Repositories;

public abstract class Repository<TEntity>(ICSGameLauncherDbContext dbContext) :
    IRepository<TEntity> where TEntity : class, IEntity
{
    protected ICSGameLauncherDbContext DbContext { get; } = dbContext;
    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();

    public async Task<List<TEntity>> GetAllAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        TEntity entity = await GetByIdAsync(id, true, cancellationToken);
        DbSet.Remove(entity);
    }

    public async Task<TEntity> GetByIdAsync(int id, bool trackChanges = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = trackChanges ? DbSet : DbSet.AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
               ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);
    }
}