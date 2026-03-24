using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;

namespace AiGarden.StorageS3Service.UseCases.Commands;

public sealed record UploadPlantPhotoCommand(
    Guid UserId,
    string FileName,
    string ContentType,
    byte[] Content) : ICommand<FileUploadResponse>;
