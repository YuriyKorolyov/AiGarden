using AiGarden.UserService.Core.Entities;

namespace AiGarden.UserService.Core.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(AppUser user, CancellationToken cancellationToken);
}
