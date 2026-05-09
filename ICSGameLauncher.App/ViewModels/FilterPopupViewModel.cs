using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.Common.Enums;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class FilterPopupViewModel : ObservableObject
{
    private bool _updatingOwnership;

    private readonly List<FilterOptionItemViewModel> _allCategoryOptions =
    [
        new("Action"),
        new("RPG"),
        new("Strategy"),
        new("Adventure"),
        new("Simulation"),
        new("Puzzle"),
        new("Shooter")
    ];

    private readonly List<FilterOptionItemViewModel> _allStudioOptions =
    [
        new("Rockstar Games"),
        new("Ubisoft"),
        new("Electronic Arts"),
        new("CD Projekt"),
        new("Valve"),
        new("Bethesda")
    ];

    [ObservableProperty]
    public partial ObservableCollection<FilterOptionItemViewModel> VisibleCategoryOptions { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<FilterOptionItemViewModel> VisibleStudioOptions { get; set; } = [];

    [ObservableProperty]
    public partial bool IsCategoryExpanded { get; set; }

    [ObservableProperty]
    public partial bool IsStudioExpanded { get; set; }

    [ObservableProperty]
    public partial bool Pegi3 { get; set; }

    [ObservableProperty]
    public partial bool Pegi7 { get; set; }

    [ObservableProperty]
    public partial bool Pegi12 { get; set; }

    [ObservableProperty]
    public partial bool Pegi16 { get; set; }

    [ObservableProperty]
    public partial bool Pegi18 { get; set; }

    [ObservableProperty]
    public partial bool IOwn { get; set; }

    [ObservableProperty]
    public partial bool IDontOwn { get; set; }

    public string CategoryToggleText => IsCategoryExpanded ? "Show Less" : "Show More";

    public string StudioToggleText => IsStudioExpanded ? "Show Less" : "Show More";

    public FilterPopupViewModel()
    {
        RefreshVisibleCategories();
        RefreshVisibleStudios();
    }

    [RelayCommand]
    private void ToggleCategoryExpand()
    {
        IsCategoryExpanded = !IsCategoryExpanded;
        RefreshVisibleCategories();
        OnPropertyChanged(nameof(CategoryToggleText));
    }

    [RelayCommand]
    private void ToggleStudioExpand()
    {
        IsStudioExpanded = !IsStudioExpanded;
        RefreshVisibleStudios();
        OnPropertyChanged(nameof(StudioToggleText));
    }

    [RelayCommand]
    private void ClearAll()
    {
        foreach (FilterOptionItemViewModel option in _allCategoryOptions)
        {
            option.IsSelected = false;
        }

        foreach (FilterOptionItemViewModel option in _allStudioOptions)
        {
            option.IsSelected = false;
        }

        Pegi3 = false;
        Pegi7 = false;
        Pegi12 = false;
        Pegi16 = false;
        Pegi18 = false;
        IOwn = false;
        IDontOwn = false;
    }

    partial void OnIOwnChanged(bool value)
    {
        if (_updatingOwnership || !value)
        {
            return;
        }

        _updatingOwnership = true;
        IDontOwn = false;
        _updatingOwnership = false;
    }

    partial void OnIDontOwnChanged(bool value)
    {
        if (_updatingOwnership || !value)
        {
            return;
        }

        _updatingOwnership = true;
        IOwn = false;
        _updatingOwnership = false;
    }

    private void RefreshVisibleCategories()
    {
        IEnumerable<FilterOptionItemViewModel> source = IsCategoryExpanded
            ? _allCategoryOptions
            : GetPrioritizedCollapsedOptions(_allCategoryOptions, 3);

        VisibleCategoryOptions = new ObservableCollection<FilterOptionItemViewModel>(source);
    }

    private void RefreshVisibleStudios()
    {
        IEnumerable<FilterOptionItemViewModel> source = IsStudioExpanded
            ? _allStudioOptions
            : GetPrioritizedCollapsedOptions(_allStudioOptions, 3);

        VisibleStudioOptions = new ObservableCollection<FilterOptionItemViewModel>(source);
    }

    private static IEnumerable<FilterOptionItemViewModel> GetPrioritizedCollapsedOptions(
        IEnumerable<FilterOptionItemViewModel> options,
        int count)
    {
        List<FilterOptionItemViewModel> optionList = options.ToList();

        List<FilterOptionItemViewModel> selected = optionList
            .Where(o => o.IsSelected)
            .ToList();

        List<FilterOptionItemViewModel> unselected = optionList
            .Where(o => !o.IsSelected)
            .ToList();

        return selected.Concat(unselected).Take(count);
    }

    public List<string> GetSelectedCategoryNames()
    {
        return _allCategoryOptions
            .Where(option => option.IsSelected)
            .Select(option => option.Name)
            .ToList();
    }

    public List<string> GetSelectedStudioNames()
    {
        return _allStudioOptions
            .Where(option => option.IsSelected)
            .Select(option => option.Name)
            .ToList();
    }

    public List<PegiAge> GetSelectedPegiRatings()
    {
        var selected = new List<PegiAge>();
        if (Pegi3) selected.Add(PegiAge.Pegi3);
        if (Pegi7) selected.Add(PegiAge.Pegi7);
        if (Pegi12) selected.Add(PegiAge.Pegi12);
        if (Pegi16) selected.Add(PegiAge.Pegi16);
        if (Pegi18) selected.Add(PegiAge.Pegi18);
        return selected;
    }

    public bool? GetOwnershipFilter()
    {
        if (IOwn == IDontOwn)
        {
            return null;
        }

        return IOwn;
    }
}
