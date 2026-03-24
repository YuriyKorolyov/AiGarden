namespace AiGarden.AiService.UseCases.Abstractions;

public interface IPlantAnalysisQueue
{
    ValueTask QueueAsync(Guid analysisId, CancellationToken cancellationToken);
}
