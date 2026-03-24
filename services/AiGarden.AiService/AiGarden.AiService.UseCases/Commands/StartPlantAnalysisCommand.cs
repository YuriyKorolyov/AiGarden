using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Enums;
using AiGarden.Contracts.Responses;

namespace AiGarden.AiService.UseCases.Commands;

public sealed record StartPlantAnalysisCommand(
    Guid UserId,
    string PhotoUrl,
    string? UserPrompt,
    AiProviderType Provider,
    string? Model) : ICommand<AnalysisAcceptedResponse>;
