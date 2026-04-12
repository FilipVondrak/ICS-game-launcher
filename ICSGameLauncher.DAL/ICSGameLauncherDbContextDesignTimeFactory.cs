using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ICSGameLauncher.DAL.Exceptions;
using DotNetEnv;

namespace ICSGameLauncher.DAL;

public sealed class ICSGameLauncherDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ICSGameLauncherDbContext>
{
    public ICSGameLauncherDbContext CreateDbContext(string[] args)
    {
        Env.Load("../.env");

        var connectionString = Environment.GetEnvironmentVariable("DESIGN_TIME_DB_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new MissingConnectionStringException("DESIGN_TIME_DB_CONNECTION_STRING");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ICSGameLauncherDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new ICSGameLauncherDbContext(optionsBuilder.Options);
    }
}
