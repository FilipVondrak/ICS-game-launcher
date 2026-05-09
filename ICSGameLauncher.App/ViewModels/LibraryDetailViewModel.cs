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
    private bool _updatingSortOptions;

    [ObservableProperty] public partial LibraryDto? Library { get; set; }

    [ObservableProperty] public partial ObservableCollection<TitleDto> Titles { get; set; } = [];

    [ObservableProperty] public partial bool IsEditPopupVisible { get; set; }

    [ObservableProperty] public partial string EditedLibraryName { get; set; } = string.Empty;

    [ObservableProperty] public partial bool IsNameValidationVisible { get; set; }
    [ObservableProperty] public partial bool IsFilterPopupVisible { get; set; }

    [ObservableProperty] public partial bool IsSortPopupVisible { get; set; }
    [ObservableProperty] public partial bool SortByName { get; set; } = true;
    [ObservableProperty] public partial bool SortByStudio { get; set; }
    [ObservableProperty] public partial bool SortByPegi { get; set; }
    [ObservableProperty] public partial bool SortByCategory { get; set; }
    [ObservableProperty] public partial bool IsSortAscending { get; set; } = true;

    public double SortAscendingOpacity => IsSortAscending ? 1.0 : 0.7;
    public double SortDescendingOpacity => IsSortAscending ? 0.7 : 1.0;

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

        ApplyCurrentSort(fetchedTitles);
    }

    [RelayCommand]
    private void ToggleSortPopup()
    {
        IsSortPopupVisible = !IsSortPopupVisible;

        if (!IsSortPopupVisible && Library is not null)
        {
            LoadTitlesCommand.Execute(null);
        }
    }

    [RelayCommand]
    private void SetSortDirection(bool ascending)
    {
        if (IsSortAscending == ascending) return;
        IsSortAscending = ascending;
    }

    partial void OnSortByNameChanged(bool value) => HandleSortModeChange(value, () => { SortByStudio = false; SortByPegi = false; SortByCategory = false; });
    partial void OnSortByStudioChanged(bool value) => HandleSortModeChange(value, () => { SortByName = false; SortByPegi = false; SortByCategory = false; });
    partial void OnSortByPegiChanged(bool value) => HandleSortModeChange(value, () => { SortByName = false; SortByStudio = false; SortByCategory = false; });
    partial void OnSortByCategoryChanged(bool value) => HandleSortModeChange(value, () => { SortByName = false; SortByStudio = false; SortByPegi = false; });

    partial void OnIsSortAscendingChanged(bool value)
    {
        OnPropertyChanged(nameof(SortAscendingOpacity));
        OnPropertyChanged(nameof(SortDescendingOpacity));
    }

    private void HandleSortModeChange(bool value, Action clearOthers)
    {
        if (_updatingSortOptions || !value) return;

        _updatingSortOptions = true;
        clearOthers();
        _updatingSortOptions = false;

        if (Library is not null)
        {
            LoadTitlesCommand.Execute(null);
        }
    }

    private void ApplyCurrentSort(IEnumerable<TitleDto> titlesToSort)
    {
        IEnumerable<TitleDto> sorted;

        if (SortByStudio)
        {
            sorted = IsSortAscending
                ? titlesToSort.OrderBy(t => t.Studios?.FirstOrDefault()?.Name)
                : titlesToSort.OrderByDescending(t => t.Studios?.FirstOrDefault()?.Name);
        }
        else if (SortByPegi)
        {
            sorted = IsSortAscending
                ? titlesToSort.OrderBy(t => t.PegiRating)
                : titlesToSort.OrderByDescending(t => t.PegiRating);
        }
        else if (SortByCategory)
        {
            sorted = IsSortAscending
                ? titlesToSort.OrderBy(t => t.Categories?.FirstOrDefault()?.Name)
                : titlesToSort.OrderByDescending(t => t.Categories?.FirstOrDefault()?.Name);
        }
        else
        {
            sorted = IsSortAscending
                ? titlesToSort.OrderBy(t => t.Name)
                : titlesToSort.OrderByDescending(t => t.Name);
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Titles = new ObservableCollection<TitleDto>(sorted.ToList());
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