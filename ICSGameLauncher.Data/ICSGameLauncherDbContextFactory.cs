using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ICSGameLauncher.Data;

public class ICSGameLauncherDbContextFactory : IDesignTimeDbContextFactory<ICSGameLauncherDbContext>
{
    public ICSGameLauncherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ICSGameLauncherDbContext>();

        optionsBuilder.UseSqlite("Data Source=icsgamelauncher.db");
        return new ICSGameLauncherDbContext(optionsBuilder.Options);
    }
}