using Amazon.S3;
using Amazon.S3.Model;
using AiGarden.StorageS3Service.Core.Services;
using Microsoft.Extensions.Options;

namespace AiGarden.StorageS3Service.Infrastructure.Storage;

public sealed class SeaweedFsS3Storage(
    IAmazonS3 s3Client,
    IOptions<SeaweedFsS3Options> options) : IObjectStorage
{
    private readonly SeaweedFsS3Options _options = options.Value;

    public async Task<(string BlobKey, string PublicUrl)> UploadAsync(
        string fileName,
        string contentType,
        byte[] content,
        CancellationToken cancellationToken)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var extension = Path.GetExtension(fileName);
        var blobKey = $"plants/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{extension}";

        await using var stream = new MemoryStream(content);
        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = blobKey,
            InputStream = stream,
            ContentType = contentType
        };

        await s3Client.PutObjectAsync(request, cancellationToken);
        var publicUrl = $"{_options.PublicBaseUrl.TrimEnd('/')}/{blobKey}";
        return (blobKey, publicUrl);
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        var buckets = await s3Client.ListBucketsAsync(cancellationToken);
        var exists = buckets.Buckets.Any(bucket => string.Equals(bucket.BucketName, _options.BucketName, StringComparison.Ordinal));
        if (exists)
        {
            return;
        }

        await s3Client.PutBucketAsync(new PutBucketRequest
        {
            BucketName = _options.BucketName
        }, cancellationToken);
    }
}
