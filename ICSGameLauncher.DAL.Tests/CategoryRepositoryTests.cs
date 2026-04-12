using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.Tests;

namespace ICSGameLauncher.DAL.Tests;

public sealed class CategoryRepositoryTests : DbContextTestsBase
{
    [Fact]
    public async Task InsertAsync_AddsCategoryToDatabase()
    {
        var context = CreateDbContext();
        var repository = new CategoryRepository(context);
        var category = new CategoryEntity { Name = "Action" };

        await repository.InsertAsync(category);
        await context.SaveChangesAsync();

        var savedCategory = await repository.GetByIdAsync(category.Id);
        Assert.NotNull(savedCategory);
        Assert.Equal("Action", savedCategory.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsCorrectCategory()
    {
        var context = CreateDbContext();
        var repository = new CategoryRepository(context);

        await repository.InsertAsync(new CategoryEntity { Name = "RPG" });
        await repository.InsertAsync(new CategoryEntity { Name = "Strategy" });
        await context.SaveChangesAsync();

        var result = await repository.GetByNameAsync("RPG");

        Assert.NotNull(result);
        Assert.Equal("RPG", result.Name);
    }

    [Fact]
    public async Task GetCategoriesWithTitlesAsync_IncludesTitlesInResultSet()
    {
        var context = CreateDbContext();
        var repository = new CategoryRepository(context);

        var title = new TitleEntity
        {
            Name = "Test Game",
            Description = "A great test game",
            PegiRating = Common.Enums.PegiAge.Pegi3
        };

        var category = new CategoryEntity { Name = "Adventure" };
        category.Titles.Add(title);

        await repository.InsertAsync(category);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var categories = await repository.GetCategoriesWithTitlesAsync();
        var adventureCategory = categories.FirstOrDefault(c => c.Name == "Adventure");

        Assert.NotNull(adventureCategory);
        Assert.Single(adventureCategory.Titles);
        Assert.Equal("Test Game", adventureCategory.Titles.First().Name);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFoundWhenCategoryIsRemoved()
    {
        var context = CreateDbContext();
        var repository = new CategoryRepository(context);
        var category = new CategoryEntity { Name = "To Be Deleted" };

        await repository.InsertAsync(category);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(category.Id);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.GetByIdAsync(category.Id));
    }
}