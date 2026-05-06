using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.DAL.UnitOfWork;

public sealed class UnitOfWorkFactory(
    IDbContextFactory<ICSGameLauncherDbContext> dbContextFactory,
    IServiceScopeFactory serviceScopeFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
        => new UnitOfWork(
            dbContext: dbContextFactory.CreateDbContext(),
            serviceScope: serviceScopeFactory.CreateScope());
}