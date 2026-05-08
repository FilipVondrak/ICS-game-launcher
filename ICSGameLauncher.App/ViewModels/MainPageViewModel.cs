using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.Services.Interfaces;
using ICSGameLauncher.BL.Facades.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserFacade _userFacade;

    [ObservableProperty] public partial ContentView CurrentContent { get; set; }
    [ObservableProperty] public partial string LoggedInUserName { get; set; }
    [ObservableProperty] public partial bool IsProfileMenuVisible { get; set; }

    private readonly Dictionary<string, ContentView> _views;


    [RelayCommand]
    private void SwitchView(string viewName)
    {
        if (_views.TryGetValue(viewName, out ContentView? view))
        {
            CurrentContent = view;
        }
    }

    [RelayCommand]
    private void ShowUserOptions()
    {
        IsProfileMenuVisible = !IsProfileMenuVisible;
    }

    [RelayCommand]
    private async Task Logout()
    {
        _currentUserService.Logout();
        if (Application.Current is not null && Application.Current.Windows.Count > 0)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }

    [RelayCommand]
    private async Task DeleteProfileAsync()
    {
        await _userFacade.DeleteUserAsync(_currentUserService.LoggedInUserId!.Value);

        await Logout();
    }

    [RelayCommand]
    private async Task EditProfileAsync()
    {
        var user = await _userFacade.GetUserAsync(_currentUserService.LoggedInUserId!.Value);
        var popupViewModel = new ProfileDetailsPopupViewModel { User = user, IsEditMode = true };

        var popupView = new ProfileDetailsPopupView { BindingContext = popupViewModel };
        var popup = new Popup { Content = popupView, CanBeDismissedByTappingOutsideOfPopup = false };

        if (Application.Current?.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            mainPage?.ShowPopup(popup);
        }

        var result = await popupViewModel.ResultPromise.Task;

        if (result != null)
        {
            result.Id = user.Id;
            await _userFacade.UpdateUserAsync(result);
            await LoadUserDataAsync();
        }

        await popup.CloseAsync();
    }

    [RelayCommand]
    private async Task ViewLoaded()
    {
        await LoadUserDataAsync();
    }

    private async Task LoadUserDataAsync()
    {
        var userId = _currentUserService.LoggedInUserId;
        if (userId is null)
        {
            LoggedInUserName = "Not found!";
            return;
        }

        var user = await _userFacade.GetUserAsync(userId.Value);
        LoggedInUserName = user.Username;
    }

    public MainPageViewModel(
        ICurrentUserService currentUserService,
        IUserFacade userFacade,
        StoreView storeView,
        LibrariesView librariesView)
    {
        _currentUserService = currentUserService;
        _userFacade = userFacade;

        _views = new Dictionary<string, ContentView> { { "Store", storeView }, { "Library", librariesView } };

        CurrentContent = _views["Store"];
    }
}