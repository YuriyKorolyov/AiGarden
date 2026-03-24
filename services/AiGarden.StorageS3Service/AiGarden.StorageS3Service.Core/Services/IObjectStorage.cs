namespace AiGarden.StorageS3Service.Core.Services;

public interface IObjectStorage
{
    Task<(string BlobKey, string PublicUrl)> UploadAsync(
        string fileName,
        string contentType,
        byte[] content,
        CancellationToken cancellationToken);
}
