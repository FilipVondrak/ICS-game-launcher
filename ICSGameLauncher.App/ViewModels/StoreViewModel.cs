using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class StoreViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITitleFacade _titleFacade;

    [ObservableProperty] public partial ObservableCollection<TitleDto> Titles { get; set; } = [];

    public StoreViewModel(IServiceProvider serviceProvider, ITitleFacade titleFacade)
    {
        _serviceProvider = serviceProvider;
        _titleFacade = titleFacade;

        LoadStoreTitlesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadStoreTitlesAsync()
    {
        var allTitles = await _titleFacade.GetAllTitlesAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Titles = new ObservableCollection<TitleDto>(allTitles);
        });
    }

    [RelayCommand]
    private static void ShowGameDetails(TitleDto game) { }

    [RelayCommand]
    private async Task AddGameToLibrary(TitleDto game)
    {
        var popupViewModel = _serviceProvider.GetRequiredService<AddToLibraryPopupViewModel>();
        popupViewModel.SelectedGame = game;
        await popupViewModel.LoadLibrariesAsync();

        var popupView = new AddToLibraryPopupView(popupViewModel);

        var popup = new Popup
        {
            Content = popupView,
            Padding = new Thickness(0),
            CanBeDismissedByTappingOutsideOfPopup = false,
            BackgroundColor = Colors.Transparent
        };

        popupViewModel.RequestClose = async () =>
        {
            await popup.CloseAsync();
        };

        if (Application.Current?.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            await mainPage!.ShowPopupAsync(popup);
        }
    }

    [RelayCommand]
    private async Task AddGame()
    {
        var viewModel = _serviceProvider.GetRequiredService<AddGamePopupViewModel>();
        var popupView = new AddGamePopupView(viewModel);

        var popup = new Popup
        {
            Content = popupView,
            Padding = new Thickness(0),
            CanBeDismissedByTappingOutsideOfPopup = false,
            BackgroundColor = Colors.Transparent
        };

        bool? isSuccess = null;
        viewModel.RequestClose = async void (result) =>
        {
            isSuccess = result;
            await popup.CloseAsync();
        };

        if (Application.Current?.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            await mainPage!.ShowPopupAsync(popup);
            if (isSuccess == true)
            {
                await LoadStoreTitlesAsync();
            }
        }
    }
}