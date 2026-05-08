using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    [RelayCommand]
    private void SwitchView(string viewName)
    {
        if (_views.TryGetValue(viewName, out ContentView? view))
        {
            CurrentContent = view;
        }
    }

    [ObservableProperty]
    public partial ContentView CurrentContent { get; set; }

    private readonly Dictionary<string, ContentView> _views = new()
    {
        { "Store", new StoreView() },
        { "Library", new LibrariesView() }
    };

    public MainPageViewModel()
    {
        CurrentContent = _views["Store"];
    }
}
