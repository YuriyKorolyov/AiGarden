using AiGarden.StorageS3Service.Core.Entities;
using AiGarden.StorageS3Service.Core.Repositories;

namespace AiGarden.StorageS3Service.Infrastructure.Persistence;

public sealed class StoredPlantPhotoRepository(StorageDbContext dbContext) : IStoredPlantPhotoRepository
{
    public Task AddAsync(StoredPlantPhoto photo, CancellationToken cancellationToken) =>
        dbContext.StoredPlantPhotos.AddAsync(photo, cancellationToken).AsTask();
}
