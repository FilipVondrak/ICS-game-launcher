namespace ICSGameLauncher.BL.Services.Interfaces;

public interface ICurrentUserService
{
    bool IsLoggedIn { get; }
    void Login(int userId);
    void Logout();
    int? LoggedInUserId { get; }
    event Action UserChanged;
}