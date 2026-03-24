using AiGarden.Contracts.Streaming;

namespace AiGarden.AiService.UseCases.Abstractions;

public interface IAnalysisEventBroker
{
    Task PublishAsync(AnalysisStreamEvent analysisEvent, CancellationToken cancellationToken);
    IAsyncEnumerable<AnalysisStreamEvent> SubscribeAsync(Guid analysisId, CancellationToken cancellationToken);
}
