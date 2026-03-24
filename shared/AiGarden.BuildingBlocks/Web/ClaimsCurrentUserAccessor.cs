using System.Security.Claims;
using AiGarden.BuildingBlocks.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AiGarden.BuildingBlocks.Web;

public sealed class ClaimsCurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Guid GetRequiredUserId()
    {
        var rawValue = httpContextAccessor.HttpContext?.User.FindFirstValue("user_id")
            ?? httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

        return Guid.TryParse(rawValue, out var userId)
            ? userId
            : DeterministicGuid.Create(rawValue ?? "anonymous");
    }

    public string? GetSubject() =>
        httpContextAccessor.HttpContext?.User.FindFirstValue("sub")
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? GetEmail() =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("email");

    public string? GetName() =>
        httpContextAccessor.HttpContext?.User.Identity?.Name
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("preferred_username")
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("name");
}
