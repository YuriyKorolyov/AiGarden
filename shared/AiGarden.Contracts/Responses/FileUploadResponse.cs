namespace AiGarden.Contracts.Responses;

public sealed record FileUploadResponse(Guid FileId, string FileName, string BlobKey, string PublicUrl);
