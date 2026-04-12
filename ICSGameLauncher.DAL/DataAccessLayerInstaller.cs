using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICSGameLauncher.DAL;

public static class DataAccessLayerInstaller
{
    public static IServiceCollection RegisterDalServices(this IServiceCollection services, string dataDirectory)
    {
        string databasePath = Path.Combine(dataDirectory, "ICSGameLauncher.db");
        string connectionString = $"Data Source={databasePath}";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new MissingConnectionStringException("DB_CONNECTION_STRING");
        }

        services.AddDbContextFactory<ICSGameLauncherDbContext>(options => options.UseSqlite(connectionString));

        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return services;
    }
}