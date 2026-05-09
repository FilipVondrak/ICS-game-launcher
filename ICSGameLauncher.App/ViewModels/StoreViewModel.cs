using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class StoreViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITitleFacade _titleFacade;
    private readonly ICurrentUserService _currentUserService;

    [ObservableProperty]
    public partial List<TitleDto> FilteredTitles { get; set; } = [];

    public StoreViewModel(
        IServiceProvider serviceProvider,
        ITitleFacade titleFacade,
        ICurrentUserService currentUserService)
    {
        _serviceProvider = serviceProvider;
        _titleFacade = titleFacade;
        _currentUserService = currentUserService;
    }

    public async Task ApplyFilterAsync(
        List<string> categoryNames,
        List<string> studioNames,
        List<ICSGameLauncher.Common.Enums.PegiAge> pegiRatings,
        bool? ownership)
    {
        FilteredTitles = await _titleFacade.GetFilteredTitlesAsync(
            categoryNames,
            studioNames,
            pegiRatings,
            ownership,
            _currentUserService.LoggedInUserId,
            libraryId: null);
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
        viewModel.RequestClose = async (result) =>
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

            }
        }
    }
}
