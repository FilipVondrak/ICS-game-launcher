namespace ICSGameLauncher.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    TRepository GetRepository<TRepository>() where TRepository : class;

    Task CommitAsync(CancellationToken cancellationToken = default);
}