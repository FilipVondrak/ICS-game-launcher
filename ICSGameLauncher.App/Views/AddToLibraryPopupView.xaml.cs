using CommunityToolkit.Maui.Views;
using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class AddToLibraryPopupView : ContentView
{
    public AddToLibraryPopupView(AddToLibraryPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}