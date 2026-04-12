using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.DAL.UnitOfWork;

public sealed class UnitOfWorkFactory(
    IDbContextFactory<ICSGameLauncherDbContext> dbContextFactory,
    IServiceScopeFactory serviceScopeFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
        => new DAL.UnitOfWork.UnitOfWork(
            dbContext: dbContextFactory.CreateDbContext(),
            serviceScope: serviceScopeFactory.CreateScope());
}