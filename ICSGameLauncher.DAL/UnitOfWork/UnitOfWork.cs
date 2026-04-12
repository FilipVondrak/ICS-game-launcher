using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.DAL.UnitOfWork;

public sealed class UnitOfWork(ICSGameLauncherDbContext dbContext, IServiceScope serviceScope) : IUnitOfWork
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly IServiceScope _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));

   public TRepository GetRepository<TRepository>() where TRepository : class
       => ActivatorUtilities.CreateInstance<TRepository>(_serviceScope.ServiceProvider, _dbContext);

   public async Task CommitAsync(CancellationToken cancellationToken = default)
       => await _dbContext.SaveChangesAsync(cancellationToken);

   public async ValueTask DisposeAsync()
   {
       await _dbContext.DisposeAsync();
       _serviceScope.Dispose();
   }
}