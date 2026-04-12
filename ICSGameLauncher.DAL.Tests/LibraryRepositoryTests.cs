using System.Threading.Tasks;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.DAL.Exceptions;
using Xunit;

namespace ICSGameLauncher.DAL.Tests;

public class LibraryRepositoryTests : ICSGameLauncher.Tests.DbContextTestsBase
{
    [Fact]
    public async Task InsertAsyncSavesLibraryToDatabase()
    {
        var user = new UserEntity() { Id = 1, Username = "Librarian", Name = "Charles", Surname = "Book", Email = "charles@book.com" };
        var library = new LibraryEntity() { Id = 1, UserId = 1, User = user, Description = "My awesome library", TitleCount = 0 };

        using (var context = CreateDbContext())
        {
            var repository = new LibraryRepository(context);
            await repository.InsertAsync(library);
            await context.SaveChangesAsync();
        }

        using (var context = CreateDbContext())
        {
            var repository = new LibraryRepository(context);
            var savedLibrary = await repository.GetByIdAsync(1);

            Assert.NotNull(savedLibrary);
            Assert.Equal("My awesome library", savedLibrary.Description);
        }
    }

    [Fact]
    public async Task DeleteAsyncRemovesLibraryAndThrowsExceptionWhenSearching()
    {
        var user = new UserEntity() { Id = 1, Username = "Librarian", Name = "Charles", Surname = "Book", Email = "charles@book.com" };
        var library = new LibraryEntity() { Id = 1, UserId = 1, User = user, Description = "Library to delete", TitleCount = 0 };

        using (var context = CreateDbContext())
        {
            var repository = new LibraryRepository(context);
            await repository.InsertAsync(library);
            await context.SaveChangesAsync();
        }

        using (var context = CreateDbContext())
        {
            var repository = new LibraryRepository(context);
            await repository.DeleteAsync(1);
            await context.SaveChangesAsync();
        }

        using (var context = CreateDbContext())
        {
            var repository = new LibraryRepository(context);

            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await repository.GetByIdAsync(1);
            });
        }
    }
}