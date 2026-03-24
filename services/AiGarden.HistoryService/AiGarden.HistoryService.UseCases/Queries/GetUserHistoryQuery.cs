using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;

namespace AiGarden.HistoryService.UseCases.Queries;

public sealed record GetUserHistoryQuery(Guid UserId) : IQuery<IReadOnlyCollection<AnalysisHistoryItemResponse>>;
