using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class AddCategoryStudioPopupView : ContentView
{
    public AddCategoryStudioPopupView(AddCategoryStudioPopupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}