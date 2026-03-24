using AiGarden.AiService.Core.Repositories;
using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;

namespace AiGarden.AiService.UseCases.Queries;

public sealed class GetPlantAnalysisQueryHandler(IPlantAnalysisRepository repository)
    : IQueryHandler<GetPlantAnalysisQuery, AnalysisResultResponse?>
{
    public async Task<AnalysisResultResponse?> HandleAsync(GetPlantAnalysisQuery query, CancellationToken cancellationToken)
    {
        var analysis = await repository.GetAsync(query.AnalysisId, cancellationToken);
        if (analysis is null)
        {
            return null;
        }

        return new AnalysisResultResponse(
            analysis.Id,
            analysis.Status,
            analysis.PhotoUrl,
            analysis.Diagnosis,
            analysis.Model,
            analysis.Provider.ToString(),
            analysis.PromptTokens,
            analysis.CompletionTokens,
            analysis.TotalTokens,
            analysis.CreatedAtUtc,
            analysis.CompletedAtUtc);
    }
}
