namespace AiGarden.BuildingBlocks.Abstractions;

public interface ICurrentUserAccessor
{
    Guid GetRequiredUserId();
    string? GetSubject();
    string? GetEmail();
    string? GetName();
}
