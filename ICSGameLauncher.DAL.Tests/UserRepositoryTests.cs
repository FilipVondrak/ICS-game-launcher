using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories;
using ICSGameLauncher.Tests;

namespace ICSGameLauncher.DAL.Tests;

public class UserRepositoryTests : DbContextTestsBase
{
    [Fact]
    public async Task InsertAsyncAddsUserToDatabase()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);
        var userEntity = new UserEntity
        {
            Username = "ondrej123",
            Name = "Ondrej",
            Surname = "Novak",
            Email = "ondrej@example.com"
        };

        await repository.InsertAsync(userEntity);
        await context.SaveChangesAsync();

        var savedUser = await repository.GetByIdAsync(userEntity.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("Ondrej", savedUser.Name);
        Assert.Equal("Novak", savedUser.Surname);
        Assert.Equal("ondrej@example.com", savedUser.Email);
    }

    [Fact]
    public async Task GetByIdAsyncReturnsExistingUser()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);
        var userEntity = new UserEntity
        {
            Username = "john123",
            Name = "John",
            Surname = "Doe",
            Email = "john@example.com"
        };

        await repository.InsertAsync(userEntity);
        await context.SaveChangesAsync();

        var loadedUser = await repository.GetByIdAsync(userEntity.Id, trackChanges: false);
        Assert.NotNull(loadedUser);
        Assert.Equal(userEntity.Id, loadedUser.Id);
        Assert.Equal("john123", loadedUser.Username);
    }

    [Fact]
    public async Task GetAllAsyncReturnsAllUsers()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);

        await repository.InsertAsync(new UserEntity
        {
            Username = "first",
            Name = "First",
            Surname = "User",
            Email = "first@example.com"
        });

        await repository.InsertAsync(new UserEntity
        {
            Username = "second",
            Name = "Second",
            Surname = "User",
            Email = "second@example.com"
        });

        await context.SaveChangesAsync();

        var users = await repository.GetAllAsync(trackChanges: false);
        Assert.Equal(2, users.Count);
    }

    [Fact]
    public async Task UpdateAsyncChangesUserFields()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);
        var userEntity = new UserEntity
        {
            Username = "oldname",
            Name = "Old",
            Surname = "User",
            Email = "old@example.com"
        };

        await repository.InsertAsync(userEntity);
        await context.SaveChangesAsync();

        userEntity.Name = "New";
        userEntity.Email = "new@example.com";

        await repository.UpdateAsync(userEntity);
        await context.SaveChangesAsync();

        var updatedUser = await repository.GetByIdAsync(userEntity.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("New", updatedUser.Name);
        Assert.Equal("new@example.com", updatedUser.Email);
    }

    [Fact]
    public async Task DeleteAsyncRemovesUser()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);
        var userEntity = new UserEntity
        {
            Username = "delete_me",
            Name = "Delete",
            Surname = "Me",
            Email = "delete@example.com"
        };

        await repository.InsertAsync(userEntity);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(userEntity.Id);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.GetByIdAsync(userEntity.Id));
    }

    [Fact]
    public async Task GetByIdAsyncWhenUserDoesNotExistThrowsEntityNotFoundException()
    {
        var context = CreateDbContext();
        var repository = new UserRepository(context);

        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.GetByIdAsync(9999));
    }
}
