using ICSGameLauncher.App.ViewModels;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace ICSGameLauncher.App.Views;

public partial class LibrariesView : ContentView
{
    private readonly LibrariesViewModel _viewModel;

    public ICommand DeleteLibraryCommand => _viewModel.DeleteLibraryCommand;
    public ICommand EditLibraryCommand => _viewModel.EditLibraryCommand;
    public ICommand OpenCreateLibraryCommand => _viewModel.OpenCreateLibraryCommand;
    public ICommand CancelCreateLibraryCommand => _viewModel.CancelCreateLibraryCommand;
    public ICommand ConfirmCreateLibraryCommand => _viewModel.ConfirmCreateLibraryCommand;

    public LibrariesView(LibrariesViewModel viewModel)
    {
        BindingContext = _viewModel = viewModel;
        InitializeComponent();
    }
}
