namespace AiGarden.Contracts.Responses;

public sealed record UserProfileResponse(Guid UserId, string Subject, string? Email, string? DisplayName);
