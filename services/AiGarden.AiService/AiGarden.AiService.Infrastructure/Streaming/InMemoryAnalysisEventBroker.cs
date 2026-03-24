using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using AiGarden.AiService.UseCases.Abstractions;
using AiGarden.Contracts.Enums;
using AiGarden.Contracts.Streaming;

namespace AiGarden.AiService.Infrastructure.Streaming;

public sealed class InMemoryAnalysisEventBroker : IAnalysisEventBroker
{
    private sealed record SubscriptionState(List<AnalysisStreamEvent> History, List<Channel<AnalysisStreamEvent>> Subscribers);

    private readonly ConcurrentDictionary<Guid, SubscriptionState> _store = new();

    public Task PublishAsync(AnalysisStreamEvent analysisEvent, CancellationToken cancellationToken)
    {
        var state = _store.GetOrAdd(analysisEvent.AnalysisId, _ => new SubscriptionState([], []));
        lock (state)
        {
            state.History.Add(analysisEvent);
            foreach (var subscriber in state.Subscribers.ToArray())
            {
                subscriber.Writer.TryWrite(analysisEvent);
                if (analysisEvent.Status is AnalysisStatus.Completed or AnalysisStatus.Failed)
                {
                    subscriber.Writer.TryComplete();
                }
            }
        }

        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<AnalysisStreamEvent> SubscribeAsync(Guid analysisId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var state = _store.GetOrAdd(analysisId, _ => new SubscriptionState([], []));
        var channel = Channel.CreateUnbounded<AnalysisStreamEvent>();
        List<AnalysisStreamEvent> history;

        lock (state)
        {
            history = state.History.ToList();
            state.Subscribers.Add(channel);
        }

        foreach (var item in history)
        {
            yield return item;
        }

        if (history.LastOrDefault()?.Status is AnalysisStatus.Completed or AnalysisStatus.Failed)
        {
            yield break;
        }

        await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken))
        {
            yield return item;
        }
    }
}
