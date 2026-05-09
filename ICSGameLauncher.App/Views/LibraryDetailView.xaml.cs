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
        bool wasVisible = FilterPopup.IsVisible;
        FilterPopup.IsVisible = !wasVisible;

        if (wasVisible)
        {
            if (BindingContext is not LibraryDetailViewModel viewModel)
            {
                return;
            }

            if (FilterPopup.BindingContext is not FilterPopupViewModel filterViewModel)
            {
                return;
            }

            _ = ApplyLibraryFilterAsync(viewModel, filterViewModel);
        }
    }

    private static async Task ApplyLibraryFilterAsync(
        LibraryDetailViewModel viewModel,
        FilterPopupViewModel filterViewModel)
    {
        await viewModel.ApplyFilterAsync(
            filterViewModel.GetSelectedCategoryNames(),
            filterViewModel.GetSelectedStudioNames(),
            filterViewModel.GetSelectedPegiRatings());
    }
}
