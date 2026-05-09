using CommunityToolkit.Mvvm.ComponentModel;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class FilterOptionItemViewModel(string name, bool isSelected = false) : ObservableObject
{
    public string Name { get; } = name;

    [ObservableProperty]
    public partial bool IsSelected { get; set; } = isSelected;
}
