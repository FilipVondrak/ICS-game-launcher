using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class AddCategoryStudioPopupViewModel(
    ICategoryFacade categoryFacade,
    IStudioFacade studioFacade
    ) : ObservableObject
{
    private readonly ICategoryFacade _categoryFacade = categoryFacade;
    private readonly IStudioFacade _studioFacade = studioFacade;
    private string _itemName;

    public Action<bool>? RequestClose { get; set; }

    [ObservableProperty] public partial string Title { get; set; }
    [ObservableProperty] public partial string InputtedName { get; set; }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(InputtedName))
        {
            RequestClose?.Invoke(false);
            return;
        }

        switch (_itemName.ToLowerInvariant())
        {
            case "category":
                var categoryDto = new CategoryDto { Name = InputtedName };
                await _categoryFacade.InsertAsync(categoryDto);
                break;
            case "studio":
                var studioDto = new StudioDto { Name = InputtedName };
                await _studioFacade.InsertAsync(studioDto);
                break;
        }
        RequestClose?.Invoke(true);
    }

    public void Initialize(string itemName)
    {
        _itemName = itemName;
        Title = $"Add new {itemName}";
    }
}