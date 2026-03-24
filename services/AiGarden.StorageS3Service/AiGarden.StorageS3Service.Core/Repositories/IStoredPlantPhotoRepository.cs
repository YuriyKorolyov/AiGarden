using AiGarden.StorageS3Service.Core.Entities;

namespace AiGarden.StorageS3Service.Core.Repositories;

public interface IStoredPlantPhotoRepository
{
    Task AddAsync(StoredPlantPhoto photo, CancellationToken cancellationToken);
}
