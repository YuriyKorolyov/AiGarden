using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;

namespace AiGarden.AiService.UseCases.Queries;

public sealed record GetPlantAnalysisQuery(Guid AnalysisId) : IQuery<AnalysisResultResponse?>;
