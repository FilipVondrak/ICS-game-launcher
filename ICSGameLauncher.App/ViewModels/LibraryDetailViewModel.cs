using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public partial class LibraryDetailViewModel : ObservableObject
{
    private readonly ITitleFacade _titleFacade;

    [ObservableProperty]
    public partial LibraryDto? Library { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<TitleDto> Titles { get; set; } = [];

    [ObservableProperty]
    public partial bool IsEditPopupVisible { get; set; }

    [ObservableProperty]
    public partial string EditedLibraryName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsNameValidationVisible { get; set; }

    [ObservableProperty]
    public partial bool IsFilterPopupVisible { get; set; }

    public LibraryDetailViewModel(ITitleFacade titleFacade)
    {
        _titleFacade = titleFacade;

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
        Titles = new ObservableCollection<TitleDto>(fetchedTitles);
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
            Id = Library.Id,
            Description = EditedLibraryName.Trim(),
            TitleCount = Library.TitleCount
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
    private static void PlayGame() {}

    [RelayCommand]
    private static void ShowGameDetails() { }

    [RelayCommand]
    private static void RemoveGame() { }

    public async Task ApplyFilterAsync(
        List<string> categoryNames,
        List<string> studioNames,
        List<ICSGameLauncher.Common.Enums.PegiAge> pegiRatings)
    {
        if (Library is null)
        {
            return;
        }

        var fetchedTitles = await _titleFacade.GetFilteredTitlesAsync(
            categoryNames,
            studioNames,
            pegiRatings,
            ownership: null,
            userId: null,
            libraryId: Library.Id);
        Titles = new ObservableCollection<TitleDto>(fetchedTitles);
    }

    [RelayCommand]
    private async Task ToggleFilterPopup(FilterPopupViewModel? filterViewModel)
    {
        bool wasVisible = IsFilterPopupVisible;
        IsFilterPopupVisible = !wasVisible;

        if (!wasVisible || filterViewModel is null)
        {
            return;
        }

        await ApplyFilterAsync(
            filterViewModel.GetSelectedCategoryNames(),
            filterViewModel.GetSelectedStudioNames(),
            filterViewModel.GetSelectedPegiRatings());
    }
}
