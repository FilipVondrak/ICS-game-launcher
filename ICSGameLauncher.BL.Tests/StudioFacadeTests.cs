using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Mappings;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

using Moq;

namespace ICSGameLauncher.BL.Tests;

public class StudioFacadeTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos_WhenEntitiesExist()
    {
        var expectedEntities = new List<StudioEntity>
        {
            new StudioEntity { Id = 1, Name = "CD Projekt Red" }, new StudioEntity { Id = 2, Name = "Mojang" }
        };

        var repositoryMock = new Mock<IStudioRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();

        uowMock
            .Setup(uow => uow.GetRepository<IStudioRepository>())
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(uow => uow.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock
            .Setup(factory => factory.Create())
            .Returns(uowMock.Object);

        var facade = new StudioFacade(factoryMock.Object);

        List<StudioDto> result = await facade.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(expectedEntities[0].Id, result[0].Id);
        Assert.Equal(expectedEntities[0].Name, result[0].Name);

        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        var expectedEntity = new StudioEntity { Id = 1, Name = "CD Projekt Red" };

        var repositoryMock = new Mock<IStudioRepository>();
        repositoryMock.Setup(repo => repo.GetByIdAsync(1, false, It.IsAny<CancellationToken>())).ReturnsAsync(expectedEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IStudioRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new StudioFacade(factoryMock.Object);

        var result = await facade.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("CD Projekt Red", result.Name);
    }

    [Fact]
    public async Task InsertAsync_ShouldCallRepositoryAndCommit()
    {
        var dtoToInsert = new StudioDto { Id = 0, Name = "New Studio" };

        var repositoryMock = new Mock<IStudioRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        uowMock.Setup(uow => uow.GetRepository<IStudioRepository>()).Returns(repositoryMock.Object);
        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new StudioFacade(factoryMock.Object);

        await facade.InsertAsync(dtoToInsert);

        repositoryMock.Verify(repo => repo.InsertAsync(It.Is<StudioEntity>(e => e.Name == "New Studio"), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallRepositoryAndCommit()
    {
        var dtoToUpdate = new StudioDto { Id = 1, Name = "Updated Studio" };
        var existingEntity = new StudioEntity { Id = 1, Name = "Old Studio" };

        var repositoryMock = new Mock<IStudioRepository>();
        repositoryMock.Setup(repo => repo.GetByIdAsync(1, true, It.IsAny<CancellationToken>())).ReturnsAsync(existingEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IStudioRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new StudioFacade(factoryMock.Object);

        await facade.UpdateAsync(dtoToUpdate);

        repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<StudioEntity>(e => e.Id == 1 && e.Name == "Updated Studio"), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryAndCommit()
    {
        var repositoryMock = new Mock<IStudioRepository>();
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IStudioRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new StudioFacade(factoryMock.Object);

        await facade.DeleteAsync(1);

        repositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}