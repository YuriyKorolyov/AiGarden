namespace AiGarden.AiService.Core.Services;

public sealed record PlantDiagnosisResult(
    string Diagnosis,
    string Model,
    int PromptTokens,
    int CompletionTokens);
