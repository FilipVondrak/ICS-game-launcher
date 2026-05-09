using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class LibrariesView : ContentView
{
    public LibrariesView(LibrariesViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
