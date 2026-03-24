using AiGarden.Contracts.Enums;

namespace AiGarden.AiService.Core.Services;

public interface IPlantDiagnosisProvider
{
    AiProviderType ProviderType { get; }

    Task<PlantDiagnosisResult> DiagnoseAsync(
        string photoUrl,
        string? userPrompt,
        string model,
        CancellationToken cancellationToken);
}
