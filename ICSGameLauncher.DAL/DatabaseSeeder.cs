using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.DAL;

public static class DatabaseSeeder
{
    public static void Seed(ICSGameLauncherDbContext dbContext)
    {
        var hasData = dbContext.Users.Any() || dbContext.Categories.Any() || dbContext.Titles.Any() ||
                      dbContext.Libraries.Any() || dbContext.Studios.Any();

        if (hasData) return;

        var studio1 = new StudioEntity { Id = 1, Name = "CD Projekt Red" };
        var studio2 = new StudioEntity { Id = 2, Name = "Rockstar Games" };

        var cat1 = new CategoryEntity { Id = 1, Name = "RPG" };
        var cat2 = new CategoryEntity { Id = 2, Name = "Závodní" };

        var title1 = new TitleEntity
        {
            Id = 1,
            Name = "Zaklínač 3: Divoký hon",
            Description = "Akční RPG s otevřeným světem.",
            PegiRating = PegiAge.Pegi18
        };
        var title2 = new TitleEntity
        {
            Id = 2,
            Name = "Forza Horizon 5",
            Description = "Závodní simulátor v Mexiku.",
            PegiRating = PegiAge.Pegi3
        };

        var user1 = new UserEntity
        {
            Id = 1,
            Username = "Hrac1",
            Name = "Jan",
            Surname = "Novák",
            Email = "novak@vut.cz"
        };
        var user2 = new UserEntity
        {
            Id = 2,
            Username = "GamerX",
            Name = "Petr",
            Surname = "Svoboda",
            Email = "svoboda@xlogin.cz"
        };

        var lib1 = new LibraryEntity { Id = 1, UserId = 1, User = user1, TitleCount = 2 };
        var lib2 = new LibraryEntity { Id = 2, UserId = 2, User = user2, TitleCount = 0 };

        dbContext.Studios.AddRange(studio1, studio2);
        dbContext.Categories.AddRange(cat1, cat2);
        dbContext.Titles.AddRange(title1, title2);
        dbContext.Users.AddRange(user1, user2);
        dbContext.Libraries.AddRange(lib1, lib2);

        dbContext.SaveChanges();

        dbContext.TitleLibraries.AddRange(
            new TitleLibraryEntity
            {
                TitleId = 1,
                LibraryId = 1,
                Title = title1,
                Library = lib1,
                LastPlayed = DateTime.Now.AddDays(-5)
            },
            new TitleLibraryEntity
            {
                TitleId = 2,
                LibraryId = 1,
                Title = title2,
                Library = lib1,
                LastPlayed = DateTime.Now.AddDays(-1)
            }
        );

        dbContext.SaveChanges();
    }
}