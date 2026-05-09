using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class StoreView : ContentView
{
    public StoreView(StoreViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
