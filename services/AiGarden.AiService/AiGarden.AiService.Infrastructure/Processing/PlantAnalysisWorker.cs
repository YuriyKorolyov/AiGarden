using AiGarden.AiService.Core.Repositories;
using AiGarden.AiService.Core.Services;
using AiGarden.AiService.Infrastructure.Persistence;
using AiGarden.AiService.UseCases.Abstractions;
using AiGarden.Contracts.Enums;
using AiGarden.Contracts.IntegrationEvents;
using AiGarden.Contracts.Streaming;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiGarden.AiService.Infrastructure.Processing;

public sealed class PlantAnalysisWorker(
    IServiceProvider serviceProvider,
    PlantAnalysisBackgroundQueue queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var analysisId in queue.ReadAllAsync(stoppingToken))
        {
            await ProcessAsync(analysisId, stoppingToken);
        }
    }

    private async Task ProcessAsync(Guid analysisId, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPlantAnalysisRepository>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AiDbContext>();
        var providers = scope.ServiceProvider.GetServices<IPlantDiagnosisProvider>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var broker = scope.ServiceProvider.GetRequiredService<IAnalysisEventBroker>();

        var analysis = await repository.GetAsync(analysisId, cancellationToken);
        if (analysis is null)
        {
            return;
        }

        try
        {
            analysis.MarkRunning();
            await dbContext.SaveChangesAsync(cancellationToken);
            await broker.PublishAsync(new AnalysisStreamEvent(analysis.Id, AnalysisStatus.Running, "Анализ запущен."), cancellationToken);

            var provider = providers.First(x => x.ProviderType == analysis.Provider);
            var result = await provider.DiagnoseAsync(analysis.PhotoUrl, analysis.UserPrompt, analysis.Model, cancellationToken);

            analysis.Complete(result.Diagnosis, result.Model, result.PromptTokens, result.CompletionTokens);
            await dbContext.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new PlantAnalysisCompletedIntegrationEvent(
                analysis.Id,
                analysis.UserId,
                analysis.PhotoUrl,
                analysis.Diagnosis ?? string.Empty,
                analysis.Provider.ToString(),
                analysis.Model,
                analysis.PromptTokens,
                analysis.CompletionTokens,
                analysis.TotalTokens,
                analysis.CreatedAtUtc,
                analysis.CompletedAtUtc ?? DateTimeOffset.UtcNow), cancellationToken);

            await broker.PublishAsync(new AnalysisStreamEvent(
                analysis.Id,
                AnalysisStatus.Completed,
                "Анализ завершён.",
                analysis.Diagnosis,
                analysis.TotalTokens,
                analysis.CompletedAtUtc), cancellationToken);
        }
        catch (Exception exception)
        {
            analysis.Fail(exception.Message);
            await dbContext.SaveChangesAsync(cancellationToken);
            await broker.PublishAsync(new AnalysisStreamEvent(analysis.Id, AnalysisStatus.Failed, $"Ошибка анализа: {exception.Message}"), cancellationToken);
        }
    }
}
