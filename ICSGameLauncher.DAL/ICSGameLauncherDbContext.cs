using ICSGameLauncher.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ICSGameLauncher.DAL;

public class ICSGameLauncherDbContext : DbContext
{
    public ICSGameLauncherDbContext(DbContextOptions<ICSGameLauncherDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Library> Libraries => Set<Library>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Studio> Studios => Set<Studio>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<TitleLibrary> TitleLibraries => Set<TitleLibrary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasMany(t => t.Libraries)
                .WithMany(l => l.Titles)
                .UsingEntity<TitleLibrary>();
        });
    }
}