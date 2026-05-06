using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Mappings;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;
using Mapster;
using Moq;

namespace ICSGameLauncher.BL.Tests;

public sealed class UserFacadeTests
{
    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnMappedDtos_WhenEntitiesExist()
    {
        var expectedEntities = new List<UserEntity>
        {
            new UserEntity
            {
                Id = 1,
                Username = "matej123",
                Name = "Matej",
                Surname = "Novak",
                Email = "matej@example.com"
            },
            new UserEntity
            {
                Id = 2,
                Username = "john456",
                Name = "John",
                Surname = "Doe",
                Email = "john@example.com"
            }
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock
            .Setup(uow => uow.GetRepository<IUserRepository>())
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(uow => uow.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock
            .Setup(factory => factory.Create())
            .Returns(uowMock.Object);

        var facade = new UserFacade(factoryMock.Object);

        List<UserDto> result = await facade.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedEntities[0].Id, result[0].Id);
        Assert.Equal(expectedEntities[0].Username, result[0].Username);
        Assert.Equal(expectedEntities[0].Name, result[0].Name);
        Assert.Equal(expectedEntities[0].Surname, result[0].Surname);
        Assert.Equal(expectedEntities[0].Email, result[0].Email);

        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        int userId = 1;
        var expectedEntity = new UserEntity
        {
            Id = userId,
            Username = "matej123",
            Name = "Matej",
            Surname = "Novak",
            Email = "matej@example.com"
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(userId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new UserFacade(factoryMock.Object);

        UserDto result = await facade.GetUserAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Username, result.Username);
        Assert.Equal(expectedEntity.Name, result.Name);
        Assert.Equal(expectedEntity.Surname, result.Surname);
        Assert.Equal(expectedEntity.Email, result.Email);

        repositoryMock.Verify(repo => repo.GetByIdAsync(userId, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldInsertEntity_AndCommitTransaction()
    {
        var newUserDto = new UserDto
        {
            Id = 3,
            Username = "newuser",
            Name = "New",
            Surname = "User",
            Email = "new.user@example.com"
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new UserFacade(factoryMock.Object);

        UserDto result = await facade.CreateUserAsync(newUserDto);

        Assert.Equal(newUserDto.Id, result.Id);
        repositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<UserEntity>(e =>
                e.Id == newUserDto.Id &&
                e.Username == newUserDto.Username &&
                e.Name == newUserDto.Name &&
                e.Surname == newUserDto.Surname &&
                e.Email == newUserDto.Email),
            It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateEntity_AndCommitTransaction()
    {
        var updatedUserDto = new UserDto
        {
            Id = 4,
            Username = "updateduser",
            Name = "Updated",
            Surname = "User",
            Email = "updated.user@example.com"
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new UserFacade(factoryMock.Object);

        UserDto result = await facade.UpdateUserAsync(updatedUserDto);

        Assert.Equal(updatedUserDto.Id, result.Id);
        repositoryMock.Verify(repo => repo.UpdateAsync(
            It.Is<UserEntity>(e =>
                e.Id == updatedUserDto.Id &&
                e.Username == updatedUserDto.Username &&
                e.Name == updatedUserDto.Name &&
                e.Surname == updatedUserDto.Surname &&
                e.Email == updatedUserDto.Email),
            It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldDeleteEntity_AndCommitTransaction()
    {
        int userIdToDelete = 1;

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repo => repo.DeleteAsync(userIdToDelete, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new UserFacade(factoryMock.Object);

        await facade.DeleteUserAsync(userIdToDelete);

        repositoryMock.Verify(repo => repo.DeleteAsync(userIdToDelete, It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
