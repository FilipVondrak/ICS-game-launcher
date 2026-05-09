using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class LibraryDetailView
{
    public LibraryDetailView(LibraryDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
