using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.DAL.UnitOfWork;

public sealed class UnitOfWorkFactory(IServiceScopeFactory serviceScopeFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        var scope = serviceScopeFactory
            .CreateScope();
        var dbContext = scope
            .ServiceProvider
            .GetRequiredService<ICSGameLauncherDbContext>();
        return new UnitOfWork(dbContext, scope);
    }
}