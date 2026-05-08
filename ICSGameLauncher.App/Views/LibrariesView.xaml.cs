using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class LibrariesView : ContentView
{
    public LibrariesView()
    {
        InitializeComponent();
        BindingContext = new LibrariesViewModel();
    }
}
