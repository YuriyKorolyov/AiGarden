namespace AiGarden.Contracts.IntegrationEvents;

public sealed record PlantAnalysisCompletedIntegrationEvent(
    Guid AnalysisId,
    Guid UserId,
    string PhotoUrl,
    string Diagnosis,
    string Provider,
    string Model,
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CompletedAtUtc);
