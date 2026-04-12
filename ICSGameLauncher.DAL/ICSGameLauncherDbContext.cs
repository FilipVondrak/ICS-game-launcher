using ICSGameLauncher.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL;

public class ICSGameLauncherDbContext : DbContext
{
    public ICSGameLauncherDbContext(DbContextOptions<ICSGameLauncherDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<LibraryEntity> Libraries => Set<LibraryEntity>();
    public DbSet<TitleEntity> Titles => Set<TitleEntity>();
    public DbSet<StudioEntity> Studios => Set<StudioEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<TitleLibraryEntity> TitleLibraries => Set<TitleLibraryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TitleEntity>(entity =>
        {
            entity.HasMany(t => t.Libraries)
                .WithMany(l => l.Titles)
                .UsingEntity<TitleLibraryEntity>();
        });
    }
}