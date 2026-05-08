using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSGameLauncher.BL.DTO;

namespace ICSGameLauncher.App.ViewModels;

public partial class ProfileDetailsPopupViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TitleText))]
    public partial bool IsEditMode { get; set; }

    public string TitleText => IsEditMode ? "Edit Profile" : "Create Profile";

    [ObservableProperty] public partial UserDto User { get; set; } = new UserDto();

    public TaskCompletionSource<UserDto?> ResultPromise { get; } = new();

    [RelayCommand]
    private void Save()
    {
        User.Username = User.Username == string.Empty ? "new user" : User.Username;
        ResultPromise.TrySetResult(User);
    }

    [RelayCommand]
    private void Cancel()
    {
        ResultPromise.TrySetResult(null);
    }
}