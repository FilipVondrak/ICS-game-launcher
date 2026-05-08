using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.DAL.Repositories.Interfaces;
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

        services.AddDbContext<ICSGameLauncherDbContext>(options => options.UseSqlite(connectionString));
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<IStudioRepository, StudioRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}