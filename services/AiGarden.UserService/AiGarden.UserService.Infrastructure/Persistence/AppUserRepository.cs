using AiGarden.UserService.Core.Entities;
using AiGarden.UserService.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.UserService.Infrastructure.Persistence;

public sealed class AppUserRepository(UserDbContext dbContext) : IAppUserRepository
{
    public Task<AppUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
        dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

    public Task AddAsync(AppUser user, CancellationToken cancellationToken) =>
        dbContext.Users.AddAsync(user, cancellationToken).AsTask();
}
