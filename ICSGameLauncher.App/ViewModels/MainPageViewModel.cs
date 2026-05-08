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

    [RelayCommand]
    private void SwitchView(string viewName)
    {
        if (_views.TryGetValue(viewName, out ContentView? view))
        {
            CurrentContent = view;
        }
    }

    [ObservableProperty] public partial ContentView CurrentContent { get; set; }
    [ObservableProperty] public partial string LoggedInUserName { get; set; }

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

    private readonly Dictionary<string, ContentView> _views = new()
    {
        { "Store", new StoreView() },
        { "Library", new LibrariesView() }
    };

    public MainPageViewModel(
        ICurrentUserService currentUserService,
        IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        CurrentContent = _views["Store"];
    }
}
