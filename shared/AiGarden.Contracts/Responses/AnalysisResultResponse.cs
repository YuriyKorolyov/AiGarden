using AiGarden.Contracts.Enums;

namespace AiGarden.Contracts.Responses;

public sealed record AnalysisResultResponse(
    Guid AnalysisId,
    AnalysisStatus Status,
    string PhotoUrl,
    string? Diagnosis,
    string? Model,
    string Provider,
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? CompletedAtUtc);
