using AiGarden.HistoryService.Core.Entities;
using AiGarden.HistoryService.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.HistoryService.Infrastructure.Persistence;

public sealed class AnalysisHistoryRepository(HistoryDbContext dbContext) : IAnalysisHistoryRepository
{
    public Task AddAsync(AnalysisHistoryEntry entry, CancellationToken cancellationToken) =>
        dbContext.HistoryEntries.AddAsync(entry, cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<AnalysisHistoryEntry>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
        await dbContext.HistoryEntries
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
}
