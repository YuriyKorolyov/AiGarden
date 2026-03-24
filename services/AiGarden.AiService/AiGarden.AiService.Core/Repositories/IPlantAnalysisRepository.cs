using AiGarden.AiService.Core.Entities;

namespace AiGarden.AiService.Core.Repositories;

public interface IPlantAnalysisRepository
{
    Task AddAsync(PlantAnalysis analysis, CancellationToken cancellationToken);
    Task<PlantAnalysis?> GetAsync(Guid analysisId, CancellationToken cancellationToken);
}
