using ICSGameLauncher.Data;
using ICSGameLauncher.Data.Models;
using Microsoft.EntityFrameworkCore;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.Tests;

public class DbContextTests : DbContextTestsBase
{
    [Fact]
    public void AddTitleWithoutRelationsSavesAndRetrievesSuccessfully()
    {
        var newTitle = new Title { Id = 1, Name = "The Witcher 3", PegiRating = PegiAge.Pegi18, Description = "RPG game" };

        using (var context = CreateDbContext())
        {
            context.Titles.Add(newTitle);
            context.SaveChanges();
        }

        using (var context = CreateDbContext())
        {
            var titleFromDb = context.Titles.FirstOrDefault(t => t.Id == newTitle.Id);

            Assert.NotNull(titleFromDb);
            Assert.Equal("The Witcher 3", titleFromDb.Name);
        }
    }

    [Fact]
    public void AddTitleWithRelatedEntitiesRetrievesAllDataUsingInclude()
    {
        var newTitle = new Title
        {
            Id = 1, Name = "The Witcher 3", PegiRating = PegiAge.Pegi18, Description = "Great RPG game full of monsters."
        };

        newTitle.Studios.Add(new Studio { Id = 1, Name = "CD Projekt Red" });
        newTitle.Categories.Add(new Category { Id = 1, Name = "RPG" });

        using (var context = CreateDbContext())
        {
            context.Titles.Add(newTitle);
            context.SaveChanges();
        }

        using (var context = CreateDbContext())
        {
            var titleFromDb = context.Titles
                .Include(t => t.Studios)
                .Include(t => t.Categories)
                .FirstOrDefault(t => t.Id == newTitle.Id);

            Assert.NotNull(titleFromDb);
            Assert.Equal("The Witcher 3", titleFromDb.Name);
            Assert.Equal("Great RPG game full of monsters.", titleFromDb.Description);
            Assert.Equal(PegiAge.Pegi18, titleFromDb.PegiRating);

            Assert.Single(titleFromDb.Studios);
            Assert.Equal("CD Projekt Red", titleFromDb.Studios.First().Name);

            Assert.Single(titleFromDb.Categories);
            Assert.Equal("RPG", titleFromDb.Categories.First().Name);
        }
    }

    [Fact]
    public void AddTitleWithAllRelationsRetrievesFullGraphSuccessfully()
    {
        var newUser = new User
        {
            Id = 1,
            Username = "Gamer123",
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@email.com"
        };

        var newLibrary = new Library
        {
            Id = 1,
            UserId = 1,
            User = newUser,
            Description = "My favorite games",
            TitleCount = 1
        };

        var newCategory = new Category { Id = 1, Name = "RPG" };
        var newStudio = new Studio { Id = 1, Name = "CD Projekt Red" };

        var newTitle = new Title
        {
            Id = 1,
            Name = "The Witcher 3",
            Description = "Masterpiece.",
            PegiRating = PegiAge.Pegi18
        };

        newTitle.Categories.Add(newCategory);
        newTitle.Studios.Add(newStudio);
        newTitle.Libraries.Add(newLibrary);

        using (var context = CreateDbContext())
        {
            context.Titles.Add(newTitle);
            context.SaveChanges();
        }

        using (var context = CreateDbContext())
        {
            var titleFromDb = context.Titles
                .Include(t => t.Categories)
                .Include(t => t.Studios)
                .Include(t => t.Libraries)
                    .ThenInclude(l => l.User)
                .FirstOrDefault(t => t.Id == newTitle.Id);

            Assert.NotNull(titleFromDb);
            Assert.Equal("The Witcher 3", titleFromDb.Name);

            Assert.Single(titleFromDb.Categories);
            Assert.Equal("RPG", titleFromDb.Categories.First().Name);

            Assert.Single(titleFromDb.Studios);
            Assert.Equal("CD Projekt Red", titleFromDb.Studios.First().Name);

            Assert.Single(titleFromDb.Libraries);
            var libraryFromDb = titleFromDb.Libraries.First();
            Assert.Equal("My favorite games", libraryFromDb.Description);
            Assert.Equal(1, libraryFromDb.TitleCount);

            Assert.NotNull(libraryFromDb.User);
            Assert.Equal("Gamer123", libraryFromDb.User.Username);
            Assert.Equal("john.doe@email.com", libraryFromDb.User.Email);
        }
    }

    [Fact]
    public void DeleteTitleWithRelationsRemovesTitleButKeepsIndependentEntities()
    {
        var category = new Category { Id = 1, Name = "Strategy" };
        var studio = new Studio { Id = 1, Name = "Firaxis" };
        var titleToDelete = new Title
        {
            Id = 1,
            Name = "Civilization VI",
            Description = "Strategy game",
            PegiRating = PegiAge.Pegi12
        };

        titleToDelete.Categories.Add(category);
        titleToDelete.Studios.Add(studio);

        using (var setupContext = CreateDbContext())
        {
            setupContext.Titles.Add(titleToDelete);
            setupContext.SaveChanges();
        }

        using (var actionContext = CreateDbContext())
        {
            var title = actionContext.Titles.First(t => t.Id == 1);
            actionContext.Titles.Remove(title);
            actionContext.SaveChanges();
        }

        using (var assertContext = CreateDbContext())
        {
            var deletedTitle = assertContext.Titles.FirstOrDefault(t => t.Id == 1);
            Assert.Null(deletedTitle);

            var survivingCategory = assertContext.Categories.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(survivingCategory);
            Assert.Equal("Strategy", survivingCategory.Name);

            var survivingStudio = assertContext.Studios.FirstOrDefault(s => s.Id == 1);
            Assert.NotNull(survivingStudio);
            Assert.Equal("Firaxis", survivingStudio.Name);
        }
    }

    [Fact]
    public void UpdateTitleChangePropertiesAndRelationsSavesChangesCorrectly()
    {
        var initialCategory = new Category { Id = 1, Name = "RPG" };
        var initialStudio = new Studio { Id = 1, Name = "CD Projekt Red" };
        var titleToUpdate = new Title
        {
            Id = 1,
            Name = "Old Title",
            Description = "Original description",
            PegiRating = PegiAge.Pegi12
        };
        titleToUpdate.Categories.Add(initialCategory);
        titleToUpdate.Studios.Add(initialStudio);

        using (var setupContext = CreateDbContext())
        {
            setupContext.Titles.Add(titleToUpdate);
            setupContext.SaveChanges();
        }

        using (var actionContext = CreateDbContext())
        {
            var title = actionContext.Titles
                .Include(t => t.Categories)
                .Include(t => t.Studios)
                .First(t => t.Id == 1);

            title.Name = "The Witcher 3: Wild Hunt";
            title.PegiRating = PegiAge.Pegi18;

            var newCategory = new Category { Name = "Action" };
            title.Categories.Add(newCategory);

            var studioToRemove = title.Studios.First(s => s.Id == 1);
            title.Studios.Remove(studioToRemove);

            actionContext.SaveChanges();
        }

        using (var assertContext = CreateDbContext())
        {
            var updatedTitle = assertContext.Titles
                .Include(t => t.Categories)
                .Include(t => t.Studios)
                .First(t => t.Id == 1);

            Assert.Equal("The Witcher 3: Wild Hunt", updatedTitle.Name);
            Assert.Equal(PegiAge.Pegi18, updatedTitle.PegiRating);

            Assert.Equal(2, updatedTitle.Categories.Count);
            Assert.Contains(updatedTitle.Categories, c => c.Name == "Action");
            Assert.Contains(updatedTitle.Categories, c => c.Name == "RPG");

            Assert.Empty(updatedTitle.Studios);
        }
    }

    [Fact]
    public void AddTitleLibraryWithCustomLastPlayedDateSavesAndRetrievesCorrectly()
    {
        var newUser = new User
        {
            Id = 1,
            Username = "Librarian",
            Name = "Charles",
            Surname = "Book",
            Email = "charles@book.com"
        };

        var newLibrary = new Library
        {
            Id = 1,
            UserId = 1,
            User = newUser,
            Description = "My main library",
            TitleCount = 1
        };

        var newTitle = new Title
        {
            Id = 1,
            Name = "Cyberpunk 2077",
            Description = "Sci-fi RPG",
            PegiRating = PegiAge.Pegi18
        };

        var expectedLastPlayed = new DateTime(2023, 10, 25, 18, 30, 0);

        var titleLibraryJoin = new TitleLibrary
        {
            TitleId = 1,
            LibraryId = 1,
            Title = newTitle,
            Library = newLibrary,
            LastPlayed = expectedLastPlayed
        };

        using (var context = CreateDbContext())
        {
            context.TitleLibraries.Add(titleLibraryJoin);
            context.SaveChanges();
        }

        using (var context = CreateDbContext())
        {
            var savedJoin = context.TitleLibraries
                .Include(tl => tl.Title)
                .Include(tl => tl.Library)
                .FirstOrDefault(tl => tl.Title.Name == "Cyberpunk 2077");

            Assert.NotNull(savedJoin);
            Assert.Equal(expectedLastPlayed, savedJoin.LastPlayed);

            Assert.NotNull(savedJoin.Library);
            Assert.Equal("My main library", savedJoin.Library.Description);
        }
    }
}