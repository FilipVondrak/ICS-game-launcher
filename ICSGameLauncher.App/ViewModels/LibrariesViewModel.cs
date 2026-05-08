using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public partial class LibrariesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LibraryDto> libraries = [];

    [ObservableProperty]
    private bool isEditPopupVisible;

    [ObservableProperty]
    private int selectedLibraryId;

    [ObservableProperty]
    private string editedLibraryName = string.Empty;

    [ObservableProperty]
    private bool isNameValidationVisible;

    public LibrariesViewModel()
    {
        Libraries =
        [
            new LibraryDto { Id = 1, UserId = 1, Description = "Favorites", TitleCount = 5 },
            new LibraryDto { Id = 2, UserId = 1, Description = "To Play", TitleCount = 2 },
            new LibraryDto { Id = 3, UserId = 1, Description = "Completed", TitleCount = 4 }
        ];
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
    private void DeleteLibrary(int id)
    {
        LibraryDto? library = Libraries.FirstOrDefault(l => l.Id == id);
        if (library is not null)
        {
            Libraries.Remove(library);
        }
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

        int index = Libraries.IndexOf(library);
        LibraryDto updatedLibrary = library with
        {
            Description = EditedLibraryName.Trim()
        };
        Libraries[index] = updatedLibrary;

        IsEditPopupVisible = false;
        IsNameValidationVisible = false;
    }
}
