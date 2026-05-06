using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.Tests;

namespace ICSGameLauncher.DAL.Tests;

public sealed class TitleRepositoryTests : DbContextTestsBase
{
    private static TitleEntity CreateTitle(string name, PegiAge pegi = PegiAge.Pegi12)
    {
        return new TitleEntity
        {
            Name = name,
            Description = "Test description",
            PegiRating = pegi
        };
    }

    [Fact]
    public async Task GetTitlesByNameAsync_ReturnsMatchingTitles_CaseInsensitive()
    {
        await using (var context = CreateDbContext())
        {
            context.Set<TitleEntity>().Add(CreateTitle("The Witcher 3"));
            context.Set<TitleEntity>().Add(CreateTitle("Cyberpunk 2077"));
            context.Set<TitleEntity>().Add(CreateTitle("The Witcher 2"));
            await context.SaveChangesAsync();
        }

        await using (var actContext = CreateDbContext())
        {
            var repository = new TitleRepository(actContext);

            var result = await repository.GetTitlesByNameAsync("witcher");

            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "The Witcher 3");
            Assert.Contains(result, t => t.Name == "The Witcher 2");
        }
    }

    [Fact]
    public async Task GetTitlesByPegiRatingAsync_ReturnsCorrectTitles()
    {
        await using (var context = CreateDbContext())
        {
            context.Set<TitleEntity>().Add(CreateTitle("Game A", PegiAge.Pegi18));
            context.Set<TitleEntity>().Add(CreateTitle("Game B", PegiAge.Pegi12));
            context.Set<TitleEntity>().Add(CreateTitle("Game C", PegiAge.Pegi18));
            await context.SaveChangesAsync();
        }

        await using (var actContext = CreateDbContext())
        {
            var repository = new TitleRepository(actContext);

            var result = await repository.GetTitlesByPegiRatingAsync(PegiAge.Pegi18);

            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal(PegiAge.Pegi18, t.PegiRating));
        }
    }

    [Fact]
    public async Task GetTitlesByCategoryAsync_ReturnsTitlesForGivenCategory()
    {
        int targetCategoryId;

        await using (var context = CreateDbContext())
        {
            var targetCategory = new CategoryEntity { Name = "RPG" };
            var otherCategory = new CategoryEntity { Name = "Action" };

            var title1 = CreateTitle("RPG Game");
            title1.Categories.Add(targetCategory);

            var title2 = CreateTitle("Action Game");
            title2.Categories.Add(otherCategory);

            context.Set<TitleEntity>().AddRange(title1, title2);
            await context.SaveChangesAsync();

            targetCategoryId = targetCategory.Id;
        }

        await using (var actContext = CreateDbContext())
        {
            var repository = new TitleRepository(actContext);

            var result = await repository.GetTitlesByCategoryAsync(targetCategoryId);

            Assert.Single(result);
            Assert.Equal("RPG Game", result.First().Name);
        }
    }

    [Fact]
    public async Task GetTitlesByNameAsync_ReturnsEmptyList_WhenNoMatchFound()
    {
        await using (var context = CreateDbContext())
        {
            context.Set<TitleEntity>().Add(CreateTitle("The Witcher 3"));
            await context.SaveChangesAsync();
        }

        await using (var actContext = CreateDbContext())
        {
            var repository = new TitleRepository(actContext);

            var result = await repository.GetTitlesByNameAsync("NonExistentGameName");

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetTitlesByPegiRatingAsync_ReturnsEmptyList_WhenNoTitlesMatch()
    {
        await using (var context = CreateDbContext())
        {
            context.Set<TitleEntity>().Add(CreateTitle("Family Game", PegiAge.Pegi3));
            await context.SaveChangesAsync();
        }

        await using (var actContext = CreateDbContext())
        {
            var repository = new TitleRepository(actContext);

            var result = await repository.GetTitlesByPegiRatingAsync(PegiAge.Pegi18);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetTitlesByCategoryAsync_ReturnsEmptyList_WhenCategoryDoesNotExist()
    {
        await using var actContext = CreateDbContext();
        var repository = new TitleRepository(actContext);
        int nonExistentCategoryId = 999;

        var result = await repository.GetTitlesByCategoryAsync(nonExistentCategoryId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTitleWithDetailsAsync_ThrowsEntityNotFoundException_WhenIdDoesNotExist()
    {
        await using var actContext = CreateDbContext();
        var repository = new TitleRepository(actContext);
        int nonExistentId = 999;

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => repository.GetTitleWithDetailsAsync(nonExistentId));
    }
}