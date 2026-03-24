using AiGarden.Contracts.Enums;

namespace AiGarden.Contracts.Streaming;

public sealed record AnalysisStreamEvent(
    Guid AnalysisId,
    AnalysisStatus Status,
    string Message,
    string? Diagnosis = null,
    int? TotalTokens = null,
    DateTimeOffset? CompletedAtUtc = null);
