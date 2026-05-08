using ICSGameLauncher.BL.Services.Interfaces;

namespace ICSGameLauncher.BL.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    public bool IsLoggedIn => LoggedInUserId is not null;

    public int? LoggedInUserId { get; private set; }

    public void Login(int userId)
    {
        LoggedInUserId = userId;
        UserChanged?.Invoke();
    }

    public void Logout()
    {
        LoggedInUserId = null;
        UserChanged?.Invoke();
    }

    public event Action? UserChanged;
}