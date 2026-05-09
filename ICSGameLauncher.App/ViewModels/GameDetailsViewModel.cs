using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using ICSGameLauncher.App.Messages;
using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.App.ViewModels;

public partial class GameDetailsViewModel : ObservableObject
{
    [ObservableProperty] public partial string StudioName { get; set; } = string.Empty;
    [ObservableProperty] public partial string CategoryName { get; set; } = string.Empty;
    [ObservableProperty] public partial TitleDto? Title { get; set; }
    [ObservableProperty] public partial LibraryDto? Library { get; set; }

    private readonly IServiceProvider _serviceProvider;
    private readonly ITitleFacade _titleFacade;
    private readonly ILibraryFacade _libraryFacade;

    public GameDetailsViewModel(
        IServiceProvider serviceProvider,
        ITitleFacade titleFacade,
        ILibraryFacade libraryFacade)
    {
        _serviceProvider = serviceProvider;
        _titleFacade = titleFacade;
        _libraryFacade = libraryFacade;
        WeakReferenceMessenger.Default.Register<OpenTitleMessage>(this, (_, message) =>
        {
            Title = message.Title;
            Library = message.Library;
        });
    }

    public async Task LoadInformation()
    {
        var titleDetail = await _titleFacade.GetTitleAsync(Title!.Id);
        StudioName = titleDetail.Studios?[0].Name ?? string.Empty;

        CategoryName = string.Empty;
        if (titleDetail.Categories is not null)
        {
            foreach (var category in titleDetail.Categories)
            {
                CategoryName += $"; {category.Name}";
            }
        }
    }

    [RelayCommand]
    private void Back()
    {
        if (Library is not null)
        {
            WeakReferenceMessenger.Default.Send(new OpenLibraryMessage(Library));
        }
        else
        {
            WeakReferenceMessenger.Default.Send(new OpenStoreMessage());
        }
    }

    [RelayCommand]
    private async Task Remove()
    {
        if (Title is null)
        {
            Back();
            return;
        }

        if (Library is not null)
        {
            await _libraryFacade.RemoveTitleFromLibraryAsync(Library.Id, Title.Id);
        }
        else if (Library is null)
        {
            await _titleFacade.DeleteTitleAsync(Title.Id);
        }

        Back();
    }
}