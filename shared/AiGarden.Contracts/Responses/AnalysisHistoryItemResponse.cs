namespace AiGarden.Contracts.Responses;

public sealed record AnalysisHistoryItemResponse(
    Guid Id,
    Guid UserId,
    string PhotoUrl,
    string Diagnosis,
    string Provider,
    string Model,
    int TotalTokens,
    DateTimeOffset CreatedAtUtc);
