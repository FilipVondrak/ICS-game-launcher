using System;
using System.Data.Common;
using ICSGameLauncher.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.Tests;

public abstract class DbContextTestsBase : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ICSGameLauncherDbContext> _dbContextOptions;

    protected DbContextTestsBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _dbContextOptions = new DbContextOptionsBuilder<ICSGameLauncherDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateDbContext();
        context.Database.EnsureCreated();
    }

    protected ICSGameLauncherDbContext CreateDbContext()
    {
        return new ICSGameLauncherDbContext(_dbContextOptions);
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}