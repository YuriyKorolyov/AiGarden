using AiGarden.AiService.Core.Entities;
using AiGarden.AiService.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.AiService.Infrastructure.Persistence;

public sealed class PlantAnalysisRepository(AiDbContext dbContext) : IPlantAnalysisRepository
{
    public Task AddAsync(PlantAnalysis analysis, CancellationToken cancellationToken) =>
        dbContext.PlantAnalyses.AddAsync(analysis, cancellationToken).AsTask();

    public Task<PlantAnalysis?> GetAsync(Guid analysisId, CancellationToken cancellationToken) =>
        dbContext.PlantAnalyses.FirstOrDefaultAsync(x => x.Id == analysisId, cancellationToken);
}
