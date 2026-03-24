using AiGarden.BuildingBlocks.Abstractions;

namespace AiGarden.UserService.Core.Entities;

public sealed class AppUser : AggregateRoot
{
    public string Subject { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? DisplayName { get; private set; }
    public DateTimeOffset LastSeenAtUtc { get; private set; }

    private AppUser()
    {
    }

    public AppUser(Guid id, string subject, string? email, string? displayName)
    {
        Id = id;
        Subject = subject;
        Email = email;
        DisplayName = displayName;
        LastSeenAtUtc = DateTimeOffset.UtcNow;
    }

    public void UpdateProfile(string? email, string? displayName)
    {
        Email = email;
        DisplayName = displayName;
        LastSeenAtUtc = DateTimeOffset.UtcNow;
    }
}
