using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.StorageS3Service.Core.Entities;
using AiGarden.StorageS3Service.Core.Repositories;
using AiGarden.StorageS3Service.Core.Services;

namespace AiGarden.StorageS3Service.UseCases.Commands;

public sealed class UploadPlantPhotoCommandHandler(
    IObjectStorage objectStorage,
    IStoredPlantPhotoRepository photoRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UploadPlantPhotoCommand, FileUploadResponse>
{
    public async Task<FileUploadResponse> HandleAsync(UploadPlantPhotoCommand command, CancellationToken cancellationToken)
    {
        var upload = await objectStorage.UploadAsync(
            command.FileName,
            command.ContentType,
            command.Content,
            cancellationToken);

        var photo = new StoredPlantPhoto(
            command.UserId,
            command.FileName,
            command.ContentType,
            upload.BlobKey,
            upload.PublicUrl);

        await photoRepository.AddAsync(photo, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new FileUploadResponse(photo.Id, photo.FileName, photo.BlobKey, photo.PublicUrl);
    }
}
