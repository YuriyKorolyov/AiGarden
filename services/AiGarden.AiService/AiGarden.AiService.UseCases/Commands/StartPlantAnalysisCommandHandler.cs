using AiGarden.AiService.Core.Entities;
using AiGarden.AiService.Core.Repositories;
using AiGarden.AiService.UseCases.Abstractions;
using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Enums;
using AiGarden.Contracts.Responses;

namespace AiGarden.AiService.UseCases.Commands;

public sealed class StartPlantAnalysisCommandHandler(
    IPlantAnalysisRepository repository,
    IUnitOfWork unitOfWork,
    IPlantAnalysisQueue analysisQueue) : ICommandHandler<StartPlantAnalysisCommand, AnalysisAcceptedResponse>
{
    public async Task<AnalysisAcceptedResponse> HandleAsync(StartPlantAnalysisCommand command, CancellationToken cancellationToken)
    {
        var defaultModel = command.Provider switch
        {
            AiProviderType.Nvidia => command.Model ?? "mistralai/mistral-large-3-675b-instruct-2512",
            AiProviderType.Ollama => command.Model ?? "qwen3.5:9b",
            _ => command.Model ?? "unknown"
        };

        var analysis = new PlantAnalysis(command.UserId, command.PhotoUrl, command.UserPrompt, command.Provider, defaultModel);

        await repository.AddAsync(analysis, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await analysisQueue.QueueAsync(analysis.Id, cancellationToken);

        return new AnalysisAcceptedResponse(
            analysis.Id,
            AnalysisStatus.Pending,
            $"/api/analyses/{analysis.Id}/events");
    }
}
