using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.UserService.UseCases.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiGarden.UserService.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UsersController(
    ICurrentUserAccessor currentUserAccessor,
    ICommandHandler<SyncCurrentUserCommand, UserProfileResponse> handler) : ControllerBase
{
    [HttpGet("me")]
    public async Task<UserProfileResponse> Me(CancellationToken cancellationToken)
    {
        var command = new SyncCurrentUserCommand(
            currentUserAccessor.GetRequiredUserId(),
            currentUserAccessor.GetSubject() ?? currentUserAccessor.GetRequiredUserId().ToString(),
            currentUserAccessor.GetEmail(),
            currentUserAccessor.GetName());

        return await handler.HandleAsync(command, cancellationToken);
    }
}
