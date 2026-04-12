using ICSGameLauncher.DAL.Models;

namespace ICSGameLauncher.DAL.Repositories;

public class UserRepository : Repository<UserEntity>
{
    public UserRepository(ICSGameLauncherDbContext dbContext) : base(dbContext)
    {
    }
}