using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class UserRepository : Repository<UserEntity>
{
    public UserRepository(ICSGameLauncherDbContext dbContext) : base(dbContext)
    {
    }
}