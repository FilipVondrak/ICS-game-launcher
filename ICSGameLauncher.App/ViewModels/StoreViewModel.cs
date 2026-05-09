using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class StoreViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    public StoreViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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