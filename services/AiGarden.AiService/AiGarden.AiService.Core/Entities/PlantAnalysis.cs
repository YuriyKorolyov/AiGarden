using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Enums;

namespace AiGarden.AiService.Core.Entities;

public sealed class PlantAnalysis : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string PhotoUrl { get; private set; } = string.Empty;
    public string? UserPrompt { get; private set; }
    public AiProviderType Provider { get; private set; }
    public string Model { get; private set; } = string.Empty;
    public AnalysisStatus Status { get; private set; }
    public string? Diagnosis { get; private set; }
    public int PromptTokens { get; private set; }
    public int CompletionTokens { get; private set; }
    public int TotalTokens { get; private set; }
    public string? Error { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? CompletedAtUtc { get; private set; }

    private PlantAnalysis()
    {
    }

    public PlantAnalysis(Guid userId, string photoUrl, string? userPrompt, AiProviderType provider, string model)
    {
        UserId = userId;
        PhotoUrl = photoUrl;
        UserPrompt = userPrompt;
        Provider = provider;
        Model = model;
        Status = AnalysisStatus.Pending;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkRunning() => Status = AnalysisStatus.Running;

    public void Complete(string diagnosis, string model, int promptTokens, int completionTokens)
    {
        Diagnosis = diagnosis;
        Model = model;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        TotalTokens = promptTokens + completionTokens;
        Status = AnalysisStatus.Completed;
        CompletedAtUtc = DateTimeOffset.UtcNow;
        Error = null;
    }

    public void Fail(string error)
    {
        Status = AnalysisStatus.Failed;
        Error = error;
        CompletedAtUtc = DateTimeOffset.UtcNow;
    }
}
