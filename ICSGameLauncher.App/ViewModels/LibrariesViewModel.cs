using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public partial class LibrariesViewModel : ObservableObject
{
    private readonly ILibraryFacade _libraryFacade;
    private readonly ITitleFacade _titleFacade;
    private readonly ICurrentUserService _currentUserService;

    [ObservableProperty]
    private ObservableCollection<LibraryDto> _libraries = [];

    [ObservableProperty]
    private bool _isEditPopupVisible;

    [ObservableProperty]
    private int _selectedLibraryId;

    [ObservableProperty]
    private string _editedLibraryName = string.Empty;

    [ObservableProperty]
    private bool _isNameValidationVisible;

    [ObservableProperty]
    private bool _isCreatePopupVisible;

    [ObservableProperty]
    private string _newLibraryName = string.Empty;

    [ObservableProperty]
    private bool _isCreateNameValidationVisible;

    public LibrariesViewModel(
        ILibraryFacade libraryFacade,
        ITitleFacade titleFacade,
        ICurrentUserService currentUserService)
    {
        _libraryFacade = libraryFacade;
        _titleFacade = titleFacade;
        _currentUserService = currentUserService;

        _ = LoadLibrariesAsync();
    }

    [RelayCommand]
    private async Task LoadLibrariesAsync()
    {
        if (_currentUserService.LoggedInUserId is not int userId)
        {
            Libraries.Clear();
            return;
        }

        List<LibraryDto> fetchedLibraries = await _libraryFacade.GetLibrariesByUserIdAsync(userId);
        List<LibraryDto> librariesWithCounts = [];

        foreach (LibraryDto library in fetchedLibraries)
        {
            List<TitleDto> titles = await _titleFacade.GetTitlesInLibraryAsync(library.Id);
            librariesWithCounts.Add(library with { TitleCount = titles.Count });
        }

        Libraries.Clear();
        foreach (LibraryDto library in librariesWithCounts)
        {
            Libraries.Add(library);
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

        if (_currentUserService.LoggedInUserId is not int userId)
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
}
