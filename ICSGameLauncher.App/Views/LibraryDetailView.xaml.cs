using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class LibraryDetailView
{
    public LibraryDetailView(LibraryDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnFilterButtonClicked(object sender, EventArgs e)
    {
        FilterPopup.IsVisible = !FilterPopup.IsVisible;
    }
}