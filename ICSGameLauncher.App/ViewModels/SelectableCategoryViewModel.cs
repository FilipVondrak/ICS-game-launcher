using CommunityToolkit.Mvvm.ComponentModel;

using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class SelectableCategoryViewModel : ObservableObject
{
    public CategoryDto Category { get; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    public SelectableCategoryViewModel(CategoryDto category, bool isSelected = false)
    {
        Category = category;
        IsSelected = isSelected;
    }
}