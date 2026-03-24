using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.HistoryService.Core.Repositories;

namespace AiGarden.HistoryService.UseCases.Queries;

public sealed class GetUserHistoryQueryHandler(IAnalysisHistoryRepository repository)
    : IQueryHandler<GetUserHistoryQuery, IReadOnlyCollection<AnalysisHistoryItemResponse>>
{
    public async Task<IReadOnlyCollection<AnalysisHistoryItemResponse>> HandleAsync(GetUserHistoryQuery query, CancellationToken cancellationToken)
    {
        var items = await repository.GetByUserIdAsync(query.UserId, cancellationToken);
        return items
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new AnalysisHistoryItemResponse(
                x.Id,
                x.UserId,
                x.PhotoUrl,
                x.Diagnosis,
                x.Provider,
                x.Model,
                x.TotalTokens,
                x.CreatedAtUtc))
            .ToArray();
    }
}
