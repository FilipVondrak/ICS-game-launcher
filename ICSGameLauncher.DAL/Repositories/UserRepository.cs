using ICSGameLauncher.DAL.Models;
using ICSGameLauncher.DAL.Repositories.Interfaces;

namespace ICSGameLauncher.DAL.Repositories;

public sealed class UserRepository(ICSGameLauncherDbContext dbContext) : Repository<UserEntity>(dbContext), IUserRepository;