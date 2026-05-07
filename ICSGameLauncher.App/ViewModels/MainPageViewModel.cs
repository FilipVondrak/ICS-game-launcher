using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    [RelayCommand]
    private void SwitchView(string viewName) => CurrentContent = _views[viewName];

    [ObservableProperty]
    private ContentView _currentContent;

    private readonly Dictionary<string, ContentView> _views = new()
    {
        { "Store", new StoreView() },
        { "Library", new LibraryView() }
    };

    public MainPageViewModel()
    {
        CurrentContent = _views["Store"];
    }
}