using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;

namespace AiGarden.UserService.UseCases.Commands;

public sealed record SyncCurrentUserCommand(
    Guid UserId,
    string Subject,
    string? Email,
    string? DisplayName) : ICommand<UserProfileResponse>;
