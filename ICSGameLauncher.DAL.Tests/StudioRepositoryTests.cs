using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.Tests;

namespace ICSGameLauncher.DAL.Tests;

public sealed class StudioRepositoryTests : DbContextTestsBase
{
    [Fact]
    public async Task InsertAsync_AddsStudioToDatabase()
    {
        var context = CreateDbContext();
        var repository = new StudioRepository(context);
        var studio = new StudioEntity { Name = "Epic Games" };

        await repository.InsertAsync(studio);
        await context.SaveChangesAsync();

        var savedStudio = await repository.GetByIdAsync(studio.Id);
        Assert.NotNull(savedStudio);
        Assert.Equal("Epic Games", savedStudio.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsCorrectStudio()
    {
        var context = CreateDbContext();
        var repository = new StudioRepository(context);

        await repository.InsertAsync(new StudioEntity { Name = "Valve" });
        await repository.InsertAsync(new StudioEntity { Name = "CD Projekt Red" });
        await context.SaveChangesAsync();

        var result = await repository.GetByNameAsync("Valve");

        Assert.NotNull(result);
        Assert.Equal("Valve", result.Name);
    }

    [Fact]
    public async Task GetStudiosWithTitlesAsync_IncludesTitlesInResultSet()
    {
        var context = CreateDbContext();
        var repository = new StudioRepository(context);

        var title = new TitleEntity
        {
            Name = "Test Game",
            Description = "A great test game",
            PegiRating = Common.Enums.PegiAge.Pegi3
        };

        var studio = new StudioEntity { Name = "Naughty Dog" };
        studio.Titles.Add(title);

        await repository.InsertAsync(studio);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var studios = await repository.GetStudiosWithTitlesAsync();
        var naughtyDogStudio = studios.FirstOrDefault(s => s.Name == "Naughty Dog");

        Assert.NotNull(naughtyDogStudio);
        Assert.Single(naughtyDogStudio.Titles);
        Assert.Equal("Test Game", naughtyDogStudio.Titles.First().Name);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFoundWhenStudioIsRemoved()
    {
        var context = CreateDbContext();
        var repository = new StudioRepository(context);
        var studio = new StudioEntity { Name = "To Be Deleted Studio" };

        await repository.InsertAsync(studio);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(studio.Id);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.GetByIdAsync(studio.Id));
    }
}