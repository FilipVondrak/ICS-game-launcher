using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;
using ICSGameLauncher.App.Messages;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class LibrariesViewModel : ObservableObject
{
    private readonly ILibraryFacade _libraryFacade;
    private readonly ITitleFacade _titleFacade;
    private readonly ICurrentUserService _currentUserService;

    [ObservableProperty]
    public partial ObservableCollection<LibraryDto> Libraries { get; set; } = [];

    [ObservableProperty]
    public partial bool IsEditPopupVisible { get; set; }

    [ObservableProperty]
    private partial int SelectedLibraryId { get; set; }

    [ObservableProperty]
    public partial string EditedLibraryName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsNameValidationVisible { get; set; }

    [ObservableProperty]
    public partial bool IsCreatePopupVisible { get; set; }

    [ObservableProperty]
    public partial string NewLibraryName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsCreateNameValidationVisible { get; set; }

    [ObservableProperty]
    public partial bool IsSortPopupVisible { get; set; }

    [ObservableProperty]
    public partial bool IsFilterPopupVisible { get; set; }

    [ObservableProperty]
    public partial bool HideEmptyLibraries { get; set; }

    [ObservableProperty]
    public partial bool SortAlphabetAsc { get; set; } = true;

    [ObservableProperty]
    public partial bool SortAlphabetDesc { get; set; }

    [ObservableProperty]
    public partial bool SortTitlesAsc { get; set; }

    [ObservableProperty]
    public partial bool SortTitlesDesc { get; set; }

    private bool _updatingSortToggles;
    private List<LibraryDto> _allLibraries = [];

    public LibrariesViewModel(
        ILibraryFacade libraryFacade,
        ITitleFacade titleFacade,
        ICurrentUserService currentUserService)
    {
        _libraryFacade = libraryFacade;
        _titleFacade = titleFacade;
        _currentUserService = currentUserService;

        _ = LoadLibrariesAsync();

        WeakReferenceMessenger.Default.Register<LibraryUpdatedMessage>(this, (_, message) =>
        {
            var updatedLibrary = message.Library;
            var existingLibrary = Libraries.FirstOrDefault(l => l.Id == updatedLibrary.Id);

            if (existingLibrary != null)
            {
                int index = Libraries.IndexOf(existingLibrary);
                Libraries[index] = updatedLibrary;
            }
        });

        WeakReferenceMessenger.Default.Register<LibraryDeletedMessage>(this, (_, message) =>
        {
            var idToDelete = message.Library.Id;

            DeleteLibraryCommand.Execute(idToDelete);
        });
    }

    [RelayCommand]
    private async Task LoadLibrariesAsync()
    {
        if (_currentUserService.LoggedInUserId is not { } userId)
        {
            Libraries = new ObservableCollection<LibraryDto>();
            return;
        }

        List<LibraryDto> fetchedLibraries = await _libraryFacade.GetLibrariesByUserIdAsync(
            userId,
            hideEmpty: HideEmptyLibraries);
        List<LibraryDto> librariesWithCounts = [];

        foreach (LibraryDto library in fetchedLibraries)
        {
            List<TitleDto> titles = await _titleFacade.GetTitlesInLibraryAsync(library.Id);
            librariesWithCounts.Add(library with { TitleCount = titles.Count });
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Libraries = new ObservableCollection<LibraryDto>(librariesWithCounts);
        });

        _allLibraries = librariesWithCounts;
        ApplyCurrentSorting();
    }

    [RelayCommand]
    private void ToggleFilterPopup()
    {
        IsFilterPopupVisible = !IsFilterPopupVisible;
        if (IsFilterPopupVisible)
        {
            IsSortPopupVisible = false;
        }
    }

    [RelayCommand]
    private void EditLibrary(int id)
    {
        LibraryDto? library = Libraries.FirstOrDefault(l => l.Id == id);
        if (library is null)
        {
            return;
        }

        SelectedLibraryId = id;
        EditedLibraryName = library.Description ?? string.Empty;
        IsNameValidationVisible = false;
        IsEditPopupVisible = true;
    }

    [RelayCommand]
    private async Task DeleteLibrary(int id)
    {
        await _libraryFacade.DeleteLibraryAsync(id);
        await LoadLibrariesAsync();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditPopupVisible = false;
        IsNameValidationVisible = false;
    }

    [RelayCommand]
    private async Task ConfirmEdit()
    {
        if (string.IsNullOrWhiteSpace(EditedLibraryName))
        {
            IsNameValidationVisible = true;
            return;
        }

        LibraryDto? library = Libraries.FirstOrDefault(l => l.Id == SelectedLibraryId);
        if (library is null)
        {
            IsEditPopupVisible = false;
            IsNameValidationVisible = false;
            return;
        }

        LibraryDto updatedLibrary = library with
        {
            Description = EditedLibraryName.Trim()
        };

        await _libraryFacade.UpdateLibraryAsync(updatedLibrary);

        IsEditPopupVisible = false;
        IsNameValidationVisible = false;

        await LoadLibrariesAsync();
    }

    [RelayCommand]
    private void OpenCreateLibrary()
    {
        NewLibraryName = string.Empty;
        IsCreateNameValidationVisible = false;
        IsCreatePopupVisible = true;
    }

    [RelayCommand]
    private void CancelCreateLibrary()
    {
        IsCreatePopupVisible = false;
        IsCreateNameValidationVisible = false;
    }

    [RelayCommand]
    private async Task ConfirmCreateLibrary()
    {
        if (string.IsNullOrWhiteSpace(NewLibraryName))
        {
            IsCreateNameValidationVisible = true;
            return;
        }

        if (_currentUserService.LoggedInUserId is not { } userId)
        {
            return;
        }

        await _libraryFacade.CreateLibraryAsync(new LibraryDto
        {
            UserId = userId,
            Description = NewLibraryName.Trim(),
            TitleCount = 0
        });

        IsCreatePopupVisible = false;
        IsCreateNameValidationVisible = false;
        NewLibraryName = string.Empty;

        await LoadLibrariesAsync();
    }

    [RelayCommand]
    private static void OpenLibrary(LibraryDto? selectedLibrary)
    {
        if (selectedLibrary is not null)
        {
            WeakReferenceMessenger.Default.Send(new OpenLibraryMessage(selectedLibrary));
        }
    }

    [RelayCommand]
    private void ToggleSortPopup()
    {
        IsSortPopupVisible = !IsSortPopupVisible;
        if (IsSortPopupVisible)
        {
            IsFilterPopupVisible = false;
        }
    }

    [RelayCommand]
    private void ClearSort()
    {
        SortAlphabetAsc = true;
        SortAlphabetDesc = false;
        SortTitlesAsc = false;
        SortTitlesDesc = false;
        ApplyCurrentSorting();
    }

    [RelayCommand]
    private void ApplySort()
    {
        ApplyCurrentSorting();
        IsSortPopupVisible = false;
    }

    private void ApplyCurrentSorting()
    {
        IEnumerable<LibraryDto> source = _allLibraries;
        if (HideEmptyLibraries)
        {
            source = source.Where(l => l.TitleCount > 0);
        }

        IOrderedEnumerable<LibraryDto> ordered = SortAlphabetDesc
            ? source.OrderByDescending(l => l.Description)
            : source.OrderBy(l => l.Description);

        if (SortTitlesAsc)
        {
            ordered = ordered.ThenBy(l => l.TitleCount);
        }
        else if (SortTitlesDesc)
        {
            ordered = ordered.ThenByDescending(l => l.TitleCount);
        }

        Libraries = new ObservableCollection<LibraryDto>(ordered);
    }

    partial void OnHideEmptyLibrariesChanged(bool value)
    {
        _ = LoadLibrariesAsync();
    }

    partial void OnSortAlphabetAscChanged(bool value)
    {
        if (_updatingSortToggles || !value)
        {
            return;
        }

        _updatingSortToggles = true;
        SortAlphabetDesc = false;
        _updatingSortToggles = false;
    }

    partial void OnSortAlphabetDescChanged(bool value)
    {
        if (_updatingSortToggles || !value)
        {
            return;
        }

        _updatingSortToggles = true;
        SortAlphabetAsc = false;
        _updatingSortToggles = false;
    }

    partial void OnSortTitlesAscChanged(bool value)
    {
        if (_updatingSortToggles || !value)
        {
            return;
        }

        _updatingSortToggles = true;
        SortTitlesDesc = false;
        _updatingSortToggles = false;
    }

    partial void OnSortTitlesDescChanged(bool value)
    {
        if (_updatingSortToggles || !value)
        {
            return;
        }

        _updatingSortToggles = true;
        SortTitlesAsc = false;
        _updatingSortToggles = false;
    }
}
