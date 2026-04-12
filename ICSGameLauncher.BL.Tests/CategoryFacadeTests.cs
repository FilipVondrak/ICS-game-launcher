using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Mappings;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

using Moq;

namespace ICSGameLauncher.BL.Tests;

public class CategoryFacadeTests
{
    public CategoryFacadeTests()
    {
        MappingsConfig.Configure();
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos_WhenEntitiesExist()
    {
        var expectedEntities = new List<CategoryEntity>
        {
            new CategoryEntity { Id = 1, Name = "RPG" }, new CategoryEntity { Id = 2, Name = "Sandbox" }
        };

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();

        uowMock
            .Setup(uow => uow.GetRepository<ICategoryRepository>())
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(uow => uow.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock
            .Setup(factory => factory.Create())
            .Returns(uowMock.Object);

        var facade = new CategoryFacade(factoryMock.Object);

        List<CategoryDto> result = await facade.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(expectedEntities[0].Id, result[0].Id);
        Assert.Equal(expectedEntities[0].Name, result[0].Name);

        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        var expectedEntity = new CategoryEntity { Id = 1, Name = "RPG" };

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(repo => repo.GetByIdAsync(1, false, It.IsAny<CancellationToken>())).ReturnsAsync(expectedEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new CategoryFacade(factoryMock.Object);

        var result = await facade.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("RPG", result.Name);
    }

    [Fact]
    public async Task InsertAsync_ShouldCallRepositoryAndCommit()
    {
        var dtoToInsert = new CategoryDto { Id = 0, Name = "New Category" };

        var repositoryMock = new Mock<ICategoryRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        uowMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(repositoryMock.Object);
        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new CategoryFacade(factoryMock.Object);

        await facade.InsertAsync(dtoToInsert);

        repositoryMock.Verify(repo => repo.InsertAsync(It.Is<CategoryEntity>(e => e.Name == "New Category"), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallRepositoryAndCommit()
    {
        var dtoToUpdate = new CategoryDto { Id = 1, Name = "Updated Category" };
        var existingEntity = new CategoryEntity { Id = 1, Name = "Old Category" };

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(repo => repo.GetByIdAsync(1, true, It.IsAny<CancellationToken>())).ReturnsAsync(existingEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new CategoryFacade(factoryMock.Object);

        await facade.UpdateAsync(dtoToUpdate);

        repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CategoryEntity>(e => e.Id == 1 && e.Name == "Updated Category"), It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryAndCommit()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(repositoryMock.Object);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(f => f.Create()).Returns(uowMock.Object);

        var facade = new CategoryFacade(factoryMock.Object);

        await facade.DeleteAsync(1);

        repositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}