using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.App.Models;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.Common.Enums;
using ICSGameLauncher.Common.Extensions;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class AddGamePopupViewModel : ObservableObject
{
    private readonly IStudioFacade _studioFacade;
    private readonly ICategoryFacade _categoryFacade;
    private readonly ITitleFacade _titleFacade;

    private readonly bool AddingNew = true;

    [ObservableProperty] public partial List<StudioDto> Studios { get; set; }
    [ObservableProperty] public partial StudioDto SelectedStudio { get; set; }
    [ObservableProperty] public partial ObservableCollection<SelectableCategoryViewModel> Categories { get; set; }
    [ObservableProperty] public partial List<PegiOption> PegiRatings { get; set; }
    [ObservableProperty] public partial PegiOption SelectedPegiRating { get; set; }
    [ObservableProperty] public partial string GameTitle { get; set; }
    [ObservableProperty] public partial string GameDescription { get; set; }

    public Action<bool>? RequestClose { get; set; }

    [RelayCommand]
    private async Task LoadOptions()
    {
        PegiRatings = Enum.GetValues<PegiAge>()
            .Select(pegi => new PegiOption(pegi, pegi.GetDescription()))
            .ToList();

        Studios = await _studioFacade.GetAllAsync();
        var categoryDtos = await _categoryFacade.GetAllAsync();
        Categories = new ObservableCollection<SelectableCategoryViewModel>(
            categoryDtos.Select(
                c => new SelectableCategoryViewModel(c, isSelected: false)
                ));
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    [RelayCommand]
    private async Task Save()
    {
        var selectedCategories = Categories
            .Where(c => c.IsSelected)
            .Select(c => c.Category);

        var gameDto = new TitleDto()
        {
            Name = GameTitle,
            Description = GameDescription,
            Studio = SelectedStudio,
            Categories = selectedCategories.ToList(),
            PegiRating = SelectedPegiRating.Value,
        };

        await _titleFacade.CreateTitleAsync(gameDto);
        RequestClose?.Invoke(true);
    }

    public AddGamePopupViewModel(
        IStudioFacade studioFacade,
        ICategoryFacade categoryFacade,
        ITitleFacade titleFacade)
    {
        _titleFacade = titleFacade;
        _categoryFacade = categoryFacade;
        _studioFacade = studioFacade;
    }
}