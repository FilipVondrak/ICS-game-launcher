using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Mappings;
using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Mapster;

using Moq;

namespace ICSGameLauncher.BL.Tests;

public sealed class TitleFacadeTests
{
    public TitleFacadeTests()
    {
        MappingsConfig.Configure();
        TypeAdapterConfig.GlobalSettings.Compile();
    }

    [Fact]
    public async Task GetAllTitlesAsync_ShouldReturnMappedDtos_WhenEntitiesExist()
    {
        var expectedEntities = new List<TitleEntity>
        {
            new TitleEntity { Id = 1, Name = "The Witcher 3", PegiRating = PegiAge.Pegi18, Description = "RPG game" },
            new TitleEntity { Id = 2, Name = "Minecraft", PegiRating = PegiAge.Pegi7, Description = "Sandbox game" }
        };

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        List<TitleDto> result = await facade.GetAllTitlesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedEntities[0].Id, result[0].Id);

        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTitleAsync_ShouldReturnMappedDto_WhenEntityExists()
    {
        int titleId = 1;
        var expectedEntity = new TitleEntity
        {
            Id = titleId,
            Name = "The Witcher 3",
            PegiRating = PegiAge.Pegi18,
            Description = "RPG game"
        };

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.GetByIdAsync(titleId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntity);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        TitleDto result = await facade.GetTitleAsync(titleId);

        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Name, result.Name);

        repositoryMock.Verify(repo => repo.GetByIdAsync(titleId, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTitlesByNameAsync_ShouldReturnMappedDtos_WhenMatchFound()
    {
        string searchName = "Witcher";
        var expectedEntities = new List<TitleEntity>
        {
            new TitleEntity { Id = 1, Name = "The Witcher 3", PegiRating = PegiAge.Pegi18, Description = "RPG game" }
        };

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.GetTitlesByNameAsync(searchName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntities);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        List<TitleDto> result = await facade.GetTitlesByNameAsync(searchName);

        Assert.Single(result);
        Assert.Equal(expectedEntities[0].Name, result[0].Name);

        repositoryMock.Verify(repo => repo.GetTitlesByNameAsync(searchName, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTitleAsync_ShouldInsertEntity_AndCommitTransaction()
    {
        var newTitleDto = new TitleDto
        {
            Id = 3,
            Name = "Cyberpunk 2077",
            PegiRating = PegiAge.Pegi18,
            Description = "Sci-Fi RPG"
        };

        var repositoryMock = new Mock<ITitleRepository>();

        repositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<TitleEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);

        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        int resultId = await facade.CreateTitleAsync(newTitleDto);

        Assert.Equal(newTitleDto.Id, resultId);

        repositoryMock.Verify(repo => repo.InsertAsync(
            It.Is<TitleEntity>(e => e.Name == newTitleDto.Name && e.Description == newTitleDto.Description),
            It.IsAny<CancellationToken>()), Times.Once);

        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTitleAsync_ShouldDeleteEntity_AndCommitTransaction()
    {
        int titleIdToDelete = 1;

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.DeleteAsync(titleIdToDelete, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        await facade.DeleteTitleAsync(titleIdToDelete);

        repositoryMock.Verify(repo => repo.DeleteAsync(titleIdToDelete, It.IsAny<CancellationToken>()), Times.Once);
        
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}