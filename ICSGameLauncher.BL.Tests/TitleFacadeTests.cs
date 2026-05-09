using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.DAL.Exceptions;
using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;
using ICSGameLauncher.DAL.UnitOfWork;

using Moq;

namespace ICSGameLauncher.BL.Tests;

public sealed class TitleFacadeTests
{
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
            .Setup(repo => repo.GetTitleWithDetailsAsync(titleId, false, It.IsAny<CancellationToken>()))
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

        repositoryMock.Verify(repo => repo.GetTitleWithDetailsAsync(titleId, false, It.IsAny<CancellationToken>()), Times.Once);
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
            .Setup(repo => repo.GetTitlesByNameAsync(searchName, false, It.IsAny<CancellationToken>()))
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

        repositoryMock.Verify(repo => repo.GetTitlesByNameAsync(searchName, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTitleAsync_ShouldInsertEntity_AndCommitTransaction()
    {
        var newTitleDto = new TitleDto
        {
            Id = 3,
            Name = "Cyberpunk 2077",
            PegiRating = PegiAge.Pegi18,
            Description = "Sci-Fi RPG",
            Studios = [new StudioDto() { Id = 1, Name = "CD Projekt Red" }]
        };

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.InsertAsync(It.IsAny<TitleEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var studioRepoMock = new Mock<IStudioRepository>();
        studioRepoMock
            .Setup(repo => repo.GetByIdAsync(newTitleDto.Studios[0].Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudioEntity { Id = newTitleDto.Studios[0].Id, Name = newTitleDto.Studios[0].Name });
        var categoryRepoMock = new Mock<ICategoryRepository>();

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<IStudioRepository>()).Returns(studioRepoMock.Object);
        uowMock.Setup(uow => uow.GetRepository<ICategoryRepository>()).Returns(categoryRepoMock.Object);
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

    [Fact]
    public async Task GetAllTitlesAsync_ShouldReturnEmptyList_WhenNoEntitiesExist()
    {
        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TitleEntity>());

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        List<TitleDto> result = await facade.GetAllTitlesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
        repositoryMock.Verify(repo => repo.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTitlesByNameAsync_ShouldReturnEmptyList_WhenNoMatchFound()
    {
        string searchName = "NonExistentGame";

        var repositoryMock = new Mock<ITitleRepository>();
        repositoryMock
            .Setup(repo => repo.GetTitlesByNameAsync(searchName, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TitleEntity>());

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        List<TitleDto> result = await facade.GetTitlesByNameAsync(searchName);

        Assert.NotNull(result);
        Assert.Empty(result);
        repositoryMock.Verify(repo => repo.GetTitlesByNameAsync(searchName, false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTitleAsync_ShouldThrowException_WhenEntityDoesNotExist()
    {
        int titleIdToDelete = 999;

        var repositoryMock = new Mock<ITitleRepository>();

        repositoryMock
            .Setup(repo => repo.DeleteAsync(titleIdToDelete, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException("title", titleIdToDelete));

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => facade.DeleteTitleAsync(titleIdToDelete));

        repositoryMock.Verify(repo => repo.DeleteAsync(titleIdToDelete, It.IsAny<CancellationToken>()), Times.Once);

        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTitleAsync_ShouldUpdateEntity_AndCommitTransaction()
    {
        var titleDtoToUpdate = new TitleDto
        {
            Id = 1,
            Name = "Updated Name",
            PegiRating = PegiAge.Pegi18,
            Description = "Updated Description",
            Studios = [new StudioDto() { Id = 1, Name = "CD Projekt Red" }]
        };

        var existingEntity = new TitleEntity
        {
            Id = 1,
            Name = "Old Name",
            PegiRating = PegiAge.Pegi12,
            Description = "Old Description"
        };

        var repositoryMock = new Mock<ITitleRepository>();

        repositoryMock
            .Setup(repo => repo.GetByIdAsync(titleDtoToUpdate.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        repositoryMock
            .Setup(repo => repo.UpdateAsync(existingEntity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);
        await facade.UpdateTitleAsync(titleDtoToUpdate);

        Assert.Equal(titleDtoToUpdate.Name, existingEntity.Name);
        Assert.Equal(titleDtoToUpdate.Description, existingEntity.Description);

        repositoryMock.Verify(repo => repo.GetByIdAsync(titleDtoToUpdate.Id, true, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repo => repo.UpdateAsync(existingEntity, It.IsAny<CancellationToken>()), Times.Once);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTitleAsync_ShouldThrowException_WhenEntityDoesNotExist()
    {
        var titleDtoToUpdate = new TitleDto
        {
            Id = 999,
            Name = "NonExistent Game",
            Studios = [new StudioDto() { Id = 1, Name = "CD Projekt Red" }]
        };

        var repositoryMock = new Mock<ITitleRepository>();

        repositoryMock
            .Setup(repo => repo.GetByIdAsync(titleDtoToUpdate.Id, true, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException("title", titleDtoToUpdate.Id));

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(uow => uow.GetRepository<ITitleRepository>()).Returns(repositoryMock.Object);
        uowMock.Setup(uow => uow.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var factoryMock = new Mock<IUnitOfWorkFactory>();
        factoryMock.Setup(factory => factory.Create()).Returns(uowMock.Object);

        var facade = new TitleFacade(factoryMock.Object);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => facade.UpdateTitleAsync(titleDtoToUpdate));

        repositoryMock.Verify(repo => repo.GetByIdAsync(titleDtoToUpdate.Id, true, It.IsAny<CancellationToken>()), Times.Once);

        repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TitleEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        uowMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}