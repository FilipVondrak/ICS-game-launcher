using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public partial class FilterPopupView : ContentView
{
    public FilterPopupView()
    {
        InitializeComponent();
        BindingContext = new FilterPopupViewModel();
    }
}
