using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.App.Views;
using ICSGameLauncher.BL.Services.Interfaces;
using ICSGameLauncher.DAL.Repositories.Interfaces;

namespace ICSGameLauncher.App.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    [ObservableProperty] public partial ContentView CurrentContent { get; set; }
    [ObservableProperty] public partial string LoggedInUserName { get; set; }

    [RelayCommand]
    private void SwitchView(string viewName)
    {
        if (_views.TryGetValue(viewName, out ContentView? view))
        {
            CurrentContent = view;
        }
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
        var user = await _userRepository.GetByIdAsync(userId.Value);
        LoggedInUserName = user.Username;
    }

    private readonly Dictionary<string, ContentView> _views;

    public MainPageViewModel(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        StoreView storeView,
        LibrariesView librariesView)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;

        _views = new Dictionary<string, ContentView>
        {
            { "Store", storeView },
            { "Library", librariesView }
        };

        CurrentContent = _views["Store"];
    }
}
