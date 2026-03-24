using System.Threading.Channels;
using AiGarden.AiService.UseCases.Abstractions;

namespace AiGarden.AiService.Infrastructure.Processing;

public sealed class PlantAnalysisBackgroundQueue : IPlantAnalysisQueue
{
    private readonly Channel<Guid> _queue = Channel.CreateUnbounded<Guid>();

    public ValueTask QueueAsync(Guid analysisId, CancellationToken cancellationToken) =>
        _queue.Writer.WriteAsync(analysisId, cancellationToken);

    public IAsyncEnumerable<Guid> ReadAllAsync(CancellationToken cancellationToken) =>
        _queue.Reader.ReadAllAsync(cancellationToken);
}
