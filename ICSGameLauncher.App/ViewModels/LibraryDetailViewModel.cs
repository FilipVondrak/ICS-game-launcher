using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSGameLauncher.App.Messages;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.App.ViewModels;

public partial class LibraryDetailViewModel : ObservableObject
{
    private readonly ITitleFacade _titleFacade;
    private readonly ILibraryFacade _libraryFacade;

    [ObservableProperty] public partial LibraryDto? Library { get; set; }

    [ObservableProperty] public partial ObservableCollection<TitleDto> Titles { get; set; } = [];

    [ObservableProperty] public partial bool IsEditPopupVisible { get; set; }

    [ObservableProperty] public partial string EditedLibraryName { get; set; } = string.Empty;

    [ObservableProperty] public partial bool IsNameValidationVisible { get; set; }
    [ObservableProperty] public partial bool IsFilterPopupVisible { get; set; }

    public LibraryDetailViewModel(ITitleFacade titleFacade, ILibraryFacade libraryFacade)
    {
        _titleFacade = titleFacade;
        _libraryFacade = libraryFacade;

        WeakReferenceMessenger.Default.Register<OpenLibraryMessage>(this, (_, message) =>
        {
            Library = message.Library;

            LoadTitlesCommand.Execute(null);
        });
    }

    [RelayCommand]
    private async Task LoadTitlesAsync()
    {
        if (Library is null) return;

        var fetchedTitles = await _titleFacade.GetTitlesInLibraryAsync(Library.Id);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Titles = new ObservableCollection<TitleDto>(fetchedTitles);
        });
    }

    [RelayCommand]
    private async Task ToggleFilterPopup(FilterPopupViewModel? filterViewModel)
    {
        _activeFilterViewModel = filterViewModel ?? _activeFilterViewModel;

        bool wasVisible = IsFilterPopupVisible;
        IsFilterPopupVisible = !wasVisible;

        if (!wasVisible)
        {
            return;
        }

        await ApplyCurrentFilterAsync();
    }

    [RelayCommand]
    private void EditLibrary()
    {
        if (Library is null) return;
        EditedLibraryName = Library.Description ?? string.Empty;
        IsNameValidationVisible = false;
        IsEditPopupVisible = true;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditPopupVisible = false;
        IsNameValidationVisible = false;
    }

    [RelayCommand]
    private void ConfirmEdit()
    {
        if (Library is null) return;

        if (string.IsNullOrWhiteSpace(EditedLibraryName))
        {
            IsNameValidationVisible = true;
            return;
        }

        Library = new LibraryDto
        {
            Id = Library.Id, Description = EditedLibraryName.Trim(), TitleCount = Library.TitleCount
        };

        WeakReferenceMessenger.Default.Send(new LibraryUpdatedMessage(Library));

        IsEditPopupVisible = false;
        IsNameValidationVisible = false;
    }

    [RelayCommand]
    private void DeleteLibrary()
    {
        if (Library is null) return;

        WeakReferenceMessenger.Default.Send(new LibraryDeletedMessage(Library));
    }


    [RelayCommand]
    private static void PlayGame()
    {
        Console.WriteLine("Play game command executed");
    }

    [RelayCommand]
    private void ShowGameDetails(TitleDto title)
    {
        WeakReferenceMessenger.Default.Send(new OpenTitleMessage(title, Library!));
    }

    [RelayCommand]
    private async Task RemoveGame(TitleDto title)
    {
        if (Library is null) return;

        await _libraryFacade.RemoveTitleFromLibraryAsync(Library.Id, title.Id);

        var freshLibrary = await _libraryFacade.GetLibraryAsync(Library.Id);

        if (freshLibrary != null)
        {
            Library = freshLibrary;

            WeakReferenceMessenger.Default.Send(new LibraryUpdatedMessage(freshLibrary));
        }

        await LoadTitlesAsync();
    }

    private async Task ApplyCurrentFilterAsync()
    {
        if (Library is null)
        {
            return;
        }

        List<TitleDto> filteredTitles = await _titleFacade.GetSortedTitlesAsync(
            SortByField.Name,
            SortDirection.Ascending,
            _activeFilterViewModel?.GetSelectedCategoryNames(),
            _activeFilterViewModel?.GetSelectedStudioNames(),
            _activeFilterViewModel?.GetSelectedPegiRatings(),
            ownership: null,
            userId: null,
            libraryId: Library.Id);

        Titles = new ObservableCollection<TitleDto>(filteredTitles);
    }

    private FilterPopupViewModel? _activeFilterViewModel;
}
