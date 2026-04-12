using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;
using ICSGameLauncher.BL.Mappings;
using Mapster;
using Moq;
using Xunit;

namespace ICSGameLauncher.BL.Tests;

public sealed class LibraryFacadeTests
{
    public LibraryFacadeTests()
    {
        MappingsConfig.Configure();
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public async Task GetAllLibrariesAsync_ShouldReturnMappedDtos_WhenEntitiesExist()
    {
        var expectedEntities = new List<LibraryEntity>
        {
            new LibraryEntity { Id = 1, Description = "Main Library", UserId = 1, TitleCount = 5, User = null! },
            new LibraryEntity { Id = 2, Description = "Secondary Library", UserId = 2, TitleCount = 2, User = null! }
        };

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        List<LibraryDto> result = await facade.GetAllLibrariesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedEntities[0].Id, result[0].Id);
        Assert.Equal(expectedEntities[0].Description, result[0].Description);

        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLibraryAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        int libraryId = 1;
        var expectedEntity = new LibraryEntity
        {
            Id = libraryId,
            Description = "Main Library",
            UserId = 1,
            TitleCount = 10,
            User = null!
        };

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.GetLibraryWithDetailsAsync(libraryId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        LibraryDto? result = await facade.GetLibraryAsync(libraryId);

        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Description, result.Description);

        repositoryMock.Verify(repo => repo.GetLibraryWithDetailsAsync(libraryId, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLibrariesByUserIdAsync_ShouldReturnMappedDtos_WhenMatchFound()
    {
        int searchUserId = 1;
        var expectedEntities = new List<LibraryEntity>
        {
            new LibraryEntity { Id = 1, Description = "User's Library", UserId = searchUserId, TitleCount = 5, User = null! }
        };

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.GetLibrariesByUserIdAsync(searchUserId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        List<LibraryDto> result = await facade.GetLibrariesByUserIdAsync(searchUserId);

        Assert.Single(result);
        Assert.Equal(expectedEntities[0].Description, result[0].Description);

        repositoryMock.Verify(repo => repo.GetLibrariesByUserIdAsync(searchUserId, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateLibraryAsync_ShouldInsertEntity_AndCommitTransaction()
    {
        var newLibraryDto = new LibraryDto
        {
            Id = 3,
            Description = "Brand New Library",
            UserId = 5
        };

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<LibraryEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        int resultId = await facade.CreateLibraryAsync(newLibraryDto);

        Assert.Equal(newLibraryDto.Id, resultId);

        repositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<LibraryEntity>(e => e.Description == newLibraryDto.Description && e.UserId == newLibraryDto.UserId),
            It.IsAny<CancellationToken>()), Times.Once);

        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLibraryAsync_ShouldUpdateEntity_AndCommitTransaction()
    {
        var updateLibraryDto = new LibraryDto
        {
            Id = 1,
            Description = "Updated Library Name",
            UserId = 1
        };

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<LibraryEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        await facade.UpdateLibraryAsync(updateLibraryDto);

        repositoryMock.Verify(repo => repo.UpdateAsync(
            It.Is<LibraryEntity>(e => e.Id == updateLibraryDto.Id && e.Description == updateLibraryDto.Description),
            It.IsAny<CancellationToken>()), Times.Once);

        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteLibraryAsync_ShouldDeleteEntity_AndCommitTransaction()
    {
        int libraryIdToDelete = 1;

        var repositoryMock = new Mock<ILibraryRepository>();
        repositoryMock
            .Setup(repo => repo.DeleteAsync(libraryIdToDelete, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ILibraryRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new LibraryFacade(factoryMock.Object);

        await facade.DeleteLibraryAsync(libraryIdToDelete);

        repositoryMock.Verify(repo => repo.DeleteAsync(libraryIdToDelete, It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}