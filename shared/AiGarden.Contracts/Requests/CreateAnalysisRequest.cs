using AiGarden.Contracts.Enums;

namespace AiGarden.Contracts.Requests;

public sealed record CreateAnalysisRequest(
    string PhotoUrl,
    string? UserPrompt,
    AiProviderType Provider,
    string? Model = null);
