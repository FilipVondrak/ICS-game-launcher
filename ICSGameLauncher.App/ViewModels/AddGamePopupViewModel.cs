using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSGameLauncher.App.Models;
using ICSGameLauncher.App.Views;
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
    private readonly IServiceProvider _provider;

    private readonly bool AddingNew = true;

    [ObservableProperty] public partial List<StudioDto> Studios { get; set; }
    [ObservableProperty] public partial StudioDto SelectedStudio { get; set; }
    [ObservableProperty] public partial ObservableCollection<SelectableCategoryViewModel> Categories { get; set; }
    [ObservableProperty] public partial List<PegiOption> PegiRatings { get; set; }
    [ObservableProperty] public partial PegiOption SelectedPegiRating { get; set; }
    [ObservableProperty] public partial string GameTitle { get; set; } = String.Empty;
    [ObservableProperty] public partial string GameDescription { get; set; } = String.Empty;
    [ObservableProperty] public partial bool ErrorVisible { get; set; }

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
            .Select(c => c.Category).ToList();

        if (SelectedStudio is null ||
            string.IsNullOrWhiteSpace(GameTitle) ||
            string.IsNullOrWhiteSpace(GameDescription) ||
            selectedCategories.Count == 0)
        {
            ErrorVisible = true;
            return;
        }

        var gameDto = new TitleDto()
        {
            Name = GameTitle,
            Description = GameDescription,
            Studios = [SelectedStudio],
            Categories = selectedCategories,
            PegiRating = SelectedPegiRating.Value,
        };

        await _titleFacade.CreateTitleAsync(gameDto);
        RequestClose?.Invoke(true);
    }

    [RelayCommand]
    private async Task CreateStudio()
    {
        var viewModel = _provider.GetRequiredService<AddCategoryStudioPopupViewModel>();
        viewModel.Initialize(itemName: "studio");
        var popupView = new AddCategoryStudioPopupView(viewModel);
        await ShowCreateDialog(popupView, viewModel);
    }

    [RelayCommand]
    private async Task CreateCategory()
    {
        var viewModel = _provider.GetRequiredService<AddCategoryStudioPopupViewModel>();
        viewModel.Initialize(itemName: "category");
        var popupView = new AddCategoryStudioPopupView(viewModel);
        await ShowCreateDialog(popupView, viewModel);
    }

    private async Task ShowCreateDialog(View view, AddCategoryStudioPopupViewModel viewModel)
    {
        var popup = new Popup
        {
            Content = view,
            Padding = new Thickness(0),
            CanBeDismissedByTappingOutsideOfPopup = false,
            BackgroundColor = Colors.Transparent
        };

        bool? isSuccess = null;
        viewModel.RequestClose = async (result) =>
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
                await LoadOptions();
            }
        }
    }

    public AddGamePopupViewModel(
        IStudioFacade studioFacade,
        ICategoryFacade categoryFacade,
        IServiceProvider serviceProvider,
        ITitleFacade titleFacade)
    {
        _provider = serviceProvider;
        _titleFacade = titleFacade;
        _categoryFacade = categoryFacade;
        _studioFacade = studioFacade;
    }
}