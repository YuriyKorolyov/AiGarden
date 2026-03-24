namespace AiGarden.StorageS3Service.Infrastructure.Storage;

public sealed class SeaweedFsS3Options
{
    public const string SectionName = "SeaweedFsS3";

    public string ServiceUrl { get; init; } = "http://localhost:8333";
    public string BucketName { get; init; } = "plant-images";
    public string AccessKey { get; init; } = "seaweedfs";
    public string SecretKey { get; init; } = "seaweedfs";
    public bool ForcePathStyle { get; init; } = true;
    public string PublicBaseUrl { get; init; } = "http://localhost:8333/plant-images";
}
