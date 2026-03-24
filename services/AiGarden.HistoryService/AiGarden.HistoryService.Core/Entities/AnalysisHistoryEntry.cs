using AiGarden.BuildingBlocks.Abstractions;

namespace AiGarden.HistoryService.Core.Entities;

public sealed class AnalysisHistoryEntry : AggregateRoot
{
    public Guid UserId { get; private set; }
    public Guid AnalysisId { get; private set; }
    public string PhotoUrl { get; private set; } = string.Empty;
    public string Diagnosis { get; private set; } = string.Empty;
    public string Provider { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int PromptTokens { get; private set; }
    public int CompletionTokens { get; private set; }
    public int TotalTokens { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private AnalysisHistoryEntry()
    {
    }

    public AnalysisHistoryEntry(
        Guid userId,
        Guid analysisId,
        string photoUrl,
        string diagnosis,
        string provider,
        string model,
        int promptTokens,
        int completionTokens,
        int totalTokens,
        DateTimeOffset createdAtUtc)
    {
        UserId = userId;
        AnalysisId = analysisId;
        PhotoUrl = photoUrl;
        Diagnosis = diagnosis;
        Provider = provider;
        Model = model;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        TotalTokens = totalTokens;
        CreatedAtUtc = createdAtUtc;
    }
}
