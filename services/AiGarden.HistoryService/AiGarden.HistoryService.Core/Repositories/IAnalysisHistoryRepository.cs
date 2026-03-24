using AiGarden.HistoryService.Core.Entities;

namespace AiGarden.HistoryService.Core.Repositories;

public interface IAnalysisHistoryRepository
{
    Task AddAsync(AnalysisHistoryEntry entry, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AnalysisHistoryEntry>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
