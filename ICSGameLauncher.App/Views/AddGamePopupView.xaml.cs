using ICSGameLauncher.App.ViewModels;

namespace ICSGameLauncher.App.Views;

public sealed partial class AddGamePopupView : ContentView
{
    public AddGamePopupView(AddGamePopupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        Loaded += OnViewLoaded;
    }

    private void OnViewLoaded(object? sender, EventArgs e)
    {
        if (BindingContext is AddGamePopupViewModel viewModel)
        {
            viewModel.LoadOptionsCommand.Execute(null);
        }
    }
}