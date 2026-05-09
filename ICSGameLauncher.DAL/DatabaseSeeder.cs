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

        var studios = new List<StudioEntity>
        {
            new() { Id = 1, Name = "CD Projekt Red" },
            new() { Id = 2, Name = "Rockstar Games" },
            new() { Id = 3, Name = "Ubisoft" },
            new() { Id = 4, Name = "FromSoftware" },
            new() { Id = 5, Name = "Bethesda Game Studios" },
            new() { Id = 6, Name = "Valve" },
            new() { Id = 7, Name = "Electronic Arts" },
            new() { Id = 8, Name = "Nintendo" },
            new() { Id = 9, Name = "Blizzard Entertainment" },
            new() { Id = 10, Name = "Sony Santa Monica" }
        };

        var categories = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "RPG" },
            new() { Id = 2, Name = "Závodní" },
            new() { Id = 3, Name = "FPS" },
            new() { Id = 4, Name = "Souls-like" },
            new() { Id = 5, Name = "Strategie" },
            new() { Id = 6, Name = "Simulátor" },
            new() { Id = 7, Name = "Akční adventura" },
            new() { Id = 8, Name = "Horror" },
            new() { Id = 9, Name = "Sportovní" },
            new() { Id = 10, Name = "Platformer" }
        };

        // --- TITULY (12) s vazbami Many-to-Many ---
        var titles = new List<TitleEntity>
        {
            new()
            {
                Id = 1,
                Name = "Zaklínač 3: Divoký hon",
                Description = "Geraltovo vrcholné dobrodružství.",
                PegiRating = PegiAge.Pegi18
            },
            new()
            {
                Id = 2,
                Name = "Forza Horizon 5",
                Description = "Festival rychlosti v Mexiku.",
                PegiRating = PegiAge.Pegi3
            },
            new()
            {
                Id = 3,
                Name = "Cyberpunk 2077",
                Description = "Budoucnost je temná a neonová.",
                PegiRating = PegiAge.Pegi18
            },
            new()
            {
                Id = 4, Name = "Elden Ring", Description = "Povstaň, Neposkvrněný.", PegiRating = PegiAge.Pegi16
            },
            new()
            {
                Id = 5,
                Name = "GTA V",
                Description = "Kriminální život v Los Santos.",
                PegiRating = PegiAge.Pegi18
            },
            new()
            {
                Id = 6,
                Name = "Skyrim",
                Description = "Dovahkiin se vrací do Skyrimu.",
                PegiRating = PegiAge.Pegi16
            },
            new()
            {
                Id = 7, Name = "Half-Life: Alyx", Description = "Revoluce ve VR.", PegiRating = PegiAge.Pegi18
            },
            new()
            {
                Id = 8,
                Name = "FIFA 23",
                Description = "Fotbalová simulace pro každého.",
                PegiRating = PegiAge.Pegi3
            },
            new()
            {
                Id = 9,
                Name = "God of War",
                Description = "Kratos a Atreus v severské mytologii.",
                PegiRating = PegiAge.Pegi18
            },
            new()
            {
                Id = 10, Name = "Starcraft II", Description = "Mezigalaktická RTS.", PegiRating = PegiAge.Pegi12
            },
            new()
            {
                Id = 11,
                Name = "Super Mario Odyssey",
                Description = "Cesta kolem světa s Mariem.",
                PegiRating = PegiAge.Pegi3
            },
            new()
            {
                Id = 12,
                Name = "Diablo IV",
                Description = "Návrat do temného Sanctuary.",
                PegiRating = PegiAge.Pegi18
            }
        };

// --- RUČNÍ PROPOJENÍ VAZEB (M:N) ---
// Protože jsou kolekce readonly (mají jen getter), plníme je pomocí .Add()

// Zaklínač 3 -> CD Projekt Red, RPG
        titles[0].Studios.Add(studios[0]); // ID 1
        titles[0].Categories.Add(categories[0]); // ID 1 (RPG)

// Forza 5 -> EA, Závodní
        titles[1].Studios.Add(studios[6]); // ID 7
        titles[1].Categories.Add(categories[1]); // ID 2 (Závodní)

// Cyberpunk -> CD Projekt Red, RPG, FPS
        titles[2].Studios.Add(studios[0]);
        titles[2].Categories.Add(categories[0]);
        titles[2].Categories.Add(categories[2]); // FPS

// Elden Ring -> FromSoftware, Souls-like, RPG
        titles[3].Studios.Add(studios[3]);
        titles[3].Categories.Add(categories[3]);
        titles[3].Categories.Add(categories[0]);

// GTA V -> Rockstar, Akční adventura
        titles[4].Studios.Add(studios[1]);
        titles[4].Categories.Add(categories[6]);

// Skyrim -> Bethesda, RPG
        titles[5].Studios.Add(studios[4]);
        titles[5].Categories.Add(categories[0]);

// Half-Life -> Valve, FPS
        titles[6].Studios.Add(studios[5]);
        titles[6].Categories.Add(categories[2]);

// FIFA -> EA, Sportovní
        titles[7].Studios.Add(studios[6]);
        titles[7].Categories.Add(categories[8]);

// God of War -> Sony, Akční adventura
        titles[8].Studios.Add(studios[9]);
        titles[8].Categories.Add(categories[6]);

// Starcraft -> Blizzard, Strategie
        titles[9].Studios.Add(studios[8]);
        titles[9].Categories.Add(categories[4]);

// Mario -> Nintendo, Platformer
        titles[10].Studios.Add(studios[7]);
        titles[10].Categories.Add(categories[9]);

// Diablo -> Blizzard, RPG, Akční
        titles[11].Studios.Add(studios[8]);
        titles[11].Categories.Add(categories[0]);

        var users = new List<UserEntity>
        {
            new()
            {
                Id = 1,
                Username = "Hrac1",
                Name = "Jan",
                Surname = "Novák",
                Email = "novak@vut.cz"
            },
            new()
            {
                Id = 2,
                Username = "GamerX",
                Name = "Petr",
                Surname = "Svoboda",
                Email = "svoboda@xlogin.cz"
            },
            new()
            {
                Id = 3,
                Username = "Alena77",
                Name = "Alena",
                Surname = "Králová",
                Email = "alena@seznam.cz"
            },
            new()
            {
                Id = 4,
                Username = "Marek_P",
                Name = "Marek",
                Surname = "Pospíšil",
                Email = "marek@gmail.com"
            },
            new()
            {
                Id = 5,
                Username = "Lucie_L",
                Name = "Lucie",
                Surname = "Lámaná",
                Email = "lucie@outlook.com"
            },
            new()
            {
                Id = 6,
                Username = "RomanV",
                Name = "Roman",
                Surname = "Velký",
                Email = "roman@vut.cz"
            },
            new()
            {
                Id = 7,
                Username = "Zuzka_S",
                Name = "Zuzana",
                Surname = "Smutná",
                Email = "zuzka@centrum.cz"
            },
            new()
            {
                Id = 8,
                Username = "Tom_Killer",
                Name = "Tomáš",
                Surname = "Drsný",
                Email = "tomas@game.cz"
            },
            new()
            {
                Id = 9,
                Username = "Petra_K",
                Name = "Petra",
                Surname = "Klidná",
                Email = "petra@vut.cz"
            },
            new()
            {
                Id = 10,
                Username = "Admin",
                Name = "Karel",
                Surname = "Správce",
                Email = "admin@launcher.cz"
            }
        };

        var libraries = users.Select(u => new LibraryEntity { Id = u.Id, UserId = u.Id, User = u, TitleCount = 0 })
            .ToList();

        dbContext.Studios.AddRange(studios);
        dbContext.Categories.AddRange(categories);
        dbContext.Titles.AddRange(titles);
        dbContext.Users.AddRange(users);
        dbContext.Libraries.AddRange(libraries);

        dbContext.SaveChanges();

        var connectionData = new[]
        {
            // L1 (Jan)
            new { T = 1, L = 1, D = 1, M = 5 }, new { T = 2, L = 1, D = 4, M = 5 }, new { T = 3, L = 1, D = 28, M = 4 },

            // L2 (Petr)
            new { T = 4, L = 2, D = 15, M = 3 }, new { T = 5, L = 2, D = 8, M = 5 },
            new { T = 6, L = 2, D = 20, M = 4 },

            // L3 (Alena)
            new { T = 6, L = 3, D = 10, M = 2 }, new { T = 7, L = 3, D = 2, M = 5 }, new { T = 1, L = 3, D = 5, M = 5 },
            new { T = 8, L = 3, D = 12, M = 5 },

            // L4 (Marek)
            new { T = 9, L = 4, D = 12, M = 4 }, new { T = 10, L = 4, D = 15, M = 5 },
            new { T = 11, L = 4, D = 2, M = 3 },

            // L5 (Lucie)
            new { T = 10, L = 5, D = 9, M = 5 }, new { T = 12, L = 5, D = 1, M = 5 },
            new { T = 1, L = 5, D = 20, M = 4 },

            // L6 (Roman)
            new { T = 11, L = 6, D = 30, M = 3 }, new { T = 2, L = 6, D = 14, M = 5 },
            new { T = 3, L = 6, D = 10, M = 5 },

            // L7 (Zuzka)
            new { T = 12, L = 7, D = 1, M = 5 }, new { T = 4, L = 7, D = 22, M = 4 },
            new { T = 5, L = 7, D = 18, M = 5 },

            // L8 (Tomáš)
            new { T = 2, L = 8, D = 5, M = 4 }, new { T = 8, L = 8, D = 12, M = 5 },
            new { T = 9, L = 8, D = 25, M = 5 },

            // L9 (Petra)
            new { T = 4, L = 9, D = 7, M = 5 }, new { T = 10, L = 9, D = 3, M = 5 },
            new { T = 6, L = 9, D = 19, M = 4 },

            // L10 (Admin)
            new { T = 5, L = 10, D = 8, M = 5 }, new { T = 7, L = 10, D = 11, M = 5 },
            new { T = 12, L = 10, D = 30, M = 4 }
        };

        var titleLibraries = connectionData.Select(c => new TitleLibraryEntity
        {
            TitleId = c.T,
            LibraryId = c.L,
            Title = titles.First(t => t.Id == c.T),
            Library = libraries.First(l => l.Id == c.L),
            LastPlayed = new DateTime(2026, c.M, c.D)
        }).ToList();

        foreach (var lib in libraries)
        {
            lib.TitleCount = titleLibraries.Count(tl => tl.LibraryId == lib.Id);
        }

        dbContext.TitleLibraries.AddRange(titleLibraries);

        dbContext.SaveChanges();
    }
}