using CommunityToolkit.Mvvm.ComponentModel;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class FilterOptionItemViewModel(string name, bool isSelected = false) : ObservableObject
{
    public string Name { get; } = name;

    [ObservableProperty]
    private bool _isSelected = isSelected;
}
