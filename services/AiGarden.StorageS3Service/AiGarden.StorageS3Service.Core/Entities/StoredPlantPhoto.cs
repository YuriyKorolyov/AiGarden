using AiGarden.BuildingBlocks.Abstractions;

namespace AiGarden.StorageS3Service.Core.Entities;

public sealed class StoredPlantPhoto : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public string BlobKey { get; private set; } = string.Empty;
    public string PublicUrl { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private StoredPlantPhoto()
    {
    }

    public StoredPlantPhoto(Guid userId, string fileName, string contentType, string blobKey, string publicUrl)
    {
        UserId = userId;
        FileName = fileName;
        ContentType = contentType;
        BlobKey = blobKey;
        PublicUrl = publicUrl;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }
}
