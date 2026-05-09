using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class StoreViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITitleFacade _titleFacade;
    private readonly ICurrentUserService _currentUserService;
    private bool _updatingSortOptions;

    [ObservableProperty] public partial ObservableCollection<TitleDto> Titles { get; set; } = [];

    public StoreViewModel(
        IServiceProvider serviceProvider,
        ITitleFacade titleFacade,
        ICurrentUserService currentUserService)
    {
        _serviceProvider = serviceProvider;
        _titleFacade = titleFacade;
        _currentUserService = currentUserService;

        LoadStoreTitlesCommand.Execute(null);
    }

    [ObservableProperty]
    public partial bool IsFilterPopupVisible { get; set; }

    [ObservableProperty]
    public partial bool IsSortPopupVisible { get; set; }

    [ObservableProperty]
    public partial bool SortByName { get; set; } = true;

    [ObservableProperty]
    public partial bool SortByStudio { get; set; }

    [ObservableProperty]
    public partial bool SortByPegi { get; set; }

    [ObservableProperty]
    public partial bool SortByCategory { get; set; }

    [ObservableProperty]
    public partial bool IsSortAscending { get; set; } = true;

    public double SortAscendingOpacity => IsSortAscending ? 1.0 : 0.7;
    public double SortDescendingOpacity => IsSortAscending ? 0.7 : 1.0;

    public async Task ApplySortAsync(
        bool sortByName,
        bool sortByStudio,
        bool sortByPegi,
        bool sortByCategory,
        bool ascending)
    {
        SortByField sortBy = ResolveSortByField(sortByName, sortByStudio, sortByPegi, sortByCategory);
        SortDirection direction = ascending ? SortDirection.Ascending : SortDirection.Descending;

        List<TitleDto> sortedTitles = await _titleFacade.GetSortedTitlesAsync(
            sortBy,
            direction,
            _activeFilterViewModel?.GetSelectedCategoryNames(),
            _activeFilterViewModel?.GetSelectedStudioNames(),
            _activeFilterViewModel?.GetSelectedPegiRatings(),
            _activeFilterViewModel?.GetOwnershipFilter(),
            _currentUserService.LoggedInUserId);

        Titles = new ObservableCollection<TitleDto>(sortedTitles);
    }

    [RelayCommand]
    private async Task ToggleFilterPopup(FilterPopupViewModel? filterViewModel)
    {
        _activeFilterViewModel = filterViewModel ?? _activeFilterViewModel;

        bool wasVisible = IsFilterPopupVisible;
        IsFilterPopupVisible = !wasVisible;
        if (IsFilterPopupVisible)
        {
            IsSortPopupVisible = false;
            return;
        }

        if (wasVisible)
        {
            await ApplyCurrentSortAsync();
        }
    }

    [RelayCommand]
    private async Task ToggleSortPopup()
    {
        bool wasVisible = IsSortPopupVisible;
        IsSortPopupVisible = !wasVisible;

        if (IsSortPopupVisible)
        {
            IsFilterPopupVisible = false;
            return;
        }

        await ApplyCurrentSortAsync();
    }

    [RelayCommand]
    private void SetSortDirection(bool ascending)
    {
        if (IsSortAscending == ascending)
        {
            return;
        }

        IsSortAscending = ascending;
    }

    private async Task ApplyCurrentSortAsync()
    {
        await ApplySortAsync(
            SortByName,
            SortByStudio,
            SortByPegi,
            SortByCategory,
            IsSortAscending);
    }

    private static SortByField ResolveSortByField(
        bool sortByName,
        bool sortByStudio,
        bool sortByPegi,
        bool sortByCategory)
    {
        if (sortByStudio)
        {
            return SortByField.Studio;
        }

        if (sortByPegi)
        {
            return SortByField.PegiRating;
        }

        if (sortByCategory)
        {
            return SortByField.Category;
        }

        return SortByField.Name;
    }

    partial void OnSortByNameChanged(bool value)
    {
        HandleSortModeChange(value, () =>
        {
            SortByStudio = false;
            SortByPegi = false;
            SortByCategory = false;
        });
    }

    partial void OnSortByStudioChanged(bool value)
    {
        HandleSortModeChange(value, () =>
        {
            SortByName = false;
            SortByPegi = false;
            SortByCategory = false;
        });
    }

    partial void OnSortByPegiChanged(bool value)
    {
        HandleSortModeChange(value, () =>
        {
            SortByName = false;
            SortByStudio = false;
            SortByCategory = false;
        });
    }

    partial void OnSortByCategoryChanged(bool value)
    {
        HandleSortModeChange(value, () =>
        {
            SortByName = false;
            SortByStudio = false;
            SortByPegi = false;
        });
    }

    partial void OnIsSortAscendingChanged(bool value)
    {
        OnPropertyChanged(nameof(SortAscendingOpacity));
        OnPropertyChanged(nameof(SortDescendingOpacity));
    }

    private void HandleSortModeChange(bool value, Action clearOthers)
    {
        if (_updatingSortOptions || !value)
        {
            return;
        }

        _updatingSortOptions = true;
        clearOthers();
        _updatingSortOptions = false;

        LoadStoreTitlesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadStoreTitlesAsync()
    {
        var allTitles = await _titleFacade.GetAllTitlesAsync();
        Titles = new ObservableCollection<TitleDto>(allTitles);
    }

    [RelayCommand]
    private static void ShowGameDetails(TitleDto game) { }

    [RelayCommand]
    private async Task AddGameToLibrary(TitleDto game)
    {
        var popupViewModel = _serviceProvider.GetRequiredService<AddToLibraryPopupViewModel>();
        popupViewModel.SelectedGame = game;
        await popupViewModel.LoadLibrariesAsync();

        var popupView = new AddToLibraryPopupView(popupViewModel);

        var popup = new Popup
        {
            Content = popupView,
            Padding = new Thickness(0),
            CanBeDismissedByTappingOutsideOfPopup = false,
            BackgroundColor = Colors.Transparent
        };

        popupViewModel.RequestClose = async () =>
        {
            await popup.CloseAsync();
        };

        if (Application.Current?.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            await mainPage!.ShowPopupAsync(popup);
        }
    }

    [RelayCommand]
    private async Task AddGame()
    {
        var viewModel = _serviceProvider.GetRequiredService<AddGamePopupViewModel>();
        var popupView = new AddGamePopupView(viewModel);

        var popup = new Popup
        {
            Content = popupView,
            Padding = new Thickness(0),
            CanBeDismissedByTappingOutsideOfPopup = false,
            BackgroundColor = Colors.Transparent
        };

        bool? isSuccess = null;
        viewModel.RequestClose = async void (result) =>
        {
            isSuccess = result;
            await popup.CloseAsync();
        };

        if (Application.Current?.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            await mainPage!.ShowPopupAsync(popup);
            if (isSuccess == true)
            {
                await LoadStoreTitlesAsync();
            }
        }
    }

    private FilterPopupViewModel? _activeFilterViewModel;
}
