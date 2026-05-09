using CommunityToolkit.Mvvm.ComponentModel;

using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class SelectableCategoryViewModel : ObservableObject
{
    public CategoryDto Category { get; }

    [ObservableProperty]
    private bool _isSelected;

    public SelectableCategoryViewModel(CategoryDto category, bool isSelected = false)
    {
        Category = category;
        _isSelected = isSelected;
    }
}