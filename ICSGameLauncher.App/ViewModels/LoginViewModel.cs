using System.Collections.ObjectModel;

using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.DTO;
using ICSGameLauncher.BL.Facades.Interfaces;
using ICSGameLauncher.BL.Services.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class LoginViewModel : ObservableObject
{
    private readonly IUserFacade _userFacade;
    private readonly ICurrentUserService _currentUserService;


    public LoginViewModel(IUserFacade userFacade, ICurrentUserService currentUserService)
    {
        _userFacade = userFacade;
        _currentUserService = currentUserService;
    }

    [ObservableProperty] public partial ObservableCollection<UserDto> Users { get; set; } = new();


    [RelayCommand]
    private async Task LoadUsersAsync()
    {
        var fetchedUsers = await _userFacade.GetAllUsersAsync();

        Users.Clear();
        foreach (var user in fetchedUsers)
        {
            Users.Add(user);
        }
    }

    [RelayCommand]
    private async Task Login(UserDto user)
    {
        if (Application.Current is not null && Application.Current.Windows.Count > 0)
        {
            _currentUserService.Login(user.Id);
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    [RelayCommand]
    private async Task CreateProfile()
    {
        try
        {
            var popupViewModel = new ProfileDetailsPopupViewModel { IsEditMode = false };
            var popupView = new ProfileDetailsPopupView { BindingContext = popupViewModel };
            var popup = new Popup
            {
                Content = popupView,
                CanBeDismissedByTappingOutsideOfPopup = false,
                BackgroundColor = Colors.Transparent,
                Padding = new Thickness(0)
            };

            if (Application.Current?.Windows.Count > 0)
            {
                var mainPage = Application.Current.Windows[0].Page;
                if (mainPage != null)
                {
                    mainPage.ShowPopup(popup);
                }
            }

            var result = await popupViewModel.ResultPromise.Task;

            if (result != null)
            {
                await _userFacade.CreateUserAsync(result);
                await LoadUsersAsync();
            }

            await popup.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            throw;
        }
    }

    [RelayCommand]
    private async Task DeleteProfileAsync(UserDto user)
    {
        await _userFacade.DeleteUserAsync(user.Id);
        await LoadUsersAsync();
    }

    [RelayCommand]
    private async Task EditProfileAsync(UserDto user)
    {
        var popupViewModel = new ProfileDetailsPopupViewModel { User = user, IsEditMode = true };

        var popupView = new ProfileDetailsPopupView { BindingContext = popupViewModel };
        var popup = new Popup
        {
            Content = popupView,
            BackgroundColor = Colors.Transparent,
            CanBeDismissedByTappingOutsideOfPopup = false,
            Padding = new Thickness(0)
        };

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
            await LoadUsersAsync();
        }

        await popup.CloseAsync();
    }
}