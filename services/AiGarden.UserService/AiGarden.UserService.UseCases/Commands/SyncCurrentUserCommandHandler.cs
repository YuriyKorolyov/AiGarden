using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.UserService.Core.Entities;
using AiGarden.UserService.Core.Repositories;

namespace AiGarden.UserService.UseCases.Commands;

public sealed class SyncCurrentUserCommandHandler(
    IAppUserRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<SyncCurrentUserCommand, UserProfileResponse>
{
    public async Task<UserProfileResponse> HandleAsync(SyncCurrentUserCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            user = new AppUser(command.UserId, command.Subject, command.Email, command.DisplayName);
            await repository.AddAsync(user, cancellationToken);
        }
        else
        {
            user.UpdateProfile(command.Email, command.DisplayName);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new UserProfileResponse(user.Id, user.Subject, user.Email, user.DisplayName);
    }
}
