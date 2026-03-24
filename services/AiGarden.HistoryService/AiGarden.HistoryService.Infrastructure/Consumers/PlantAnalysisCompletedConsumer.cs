using AiGarden.Contracts.IntegrationEvents;
using AiGarden.HistoryService.Core.Entities;
using AiGarden.HistoryService.Core.Repositories;
using AiGarden.HistoryService.Infrastructure.Persistence;
using MassTransit;

namespace AiGarden.HistoryService.Infrastructure.Consumers;

public sealed class PlantAnalysisCompletedConsumer(
    IAnalysisHistoryRepository repository,
    HistoryDbContext dbContext) : IConsumer<PlantAnalysisCompletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<PlantAnalysisCompletedIntegrationEvent> context)
    {
        var message = context.Message;
        var entry = new AnalysisHistoryEntry(
            message.UserId,
            message.AnalysisId,
            message.PhotoUrl,
            message.Diagnosis,
            message.Provider,
            message.Model,
            message.PromptTokens,
            message.CompletionTokens,
            message.TotalTokens,
            message.CompletedAtUtc);

        await repository.AddAsync(entry, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
