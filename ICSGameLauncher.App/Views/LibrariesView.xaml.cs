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

    public LibrariesView()
    {
        IServiceProvider? services = Application.Current?.Handler?.MauiContext?.Services;
        if (services is null)
        {
            throw new InvalidOperationException("App services are not available.");
        }

        ILibraryFacade libraryFacade = services.GetRequiredService<ILibraryFacade>();
        ITitleFacade titleFacade = services.GetRequiredService<ITitleFacade>();
        ICurrentUserService currentUserService = services.GetRequiredService<ICurrentUserService>();
        _viewModel = new LibrariesViewModel(libraryFacade, titleFacade, currentUserService);

        BindingContext = _viewModel;
        InitializeComponent();
    }
}
