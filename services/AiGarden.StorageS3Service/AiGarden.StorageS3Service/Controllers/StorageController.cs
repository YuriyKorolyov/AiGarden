using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.Contracts.Responses;
using AiGarden.StorageS3Service.UseCases.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiGarden.StorageS3Service.Controllers;

[ApiController]
[Route("api/storage")]
[Authorize]
public sealed class StorageController(
    ICurrentUserAccessor currentUserAccessor,
    IValidator<UploadPlantPhotoCommand> validator,
    ICommandHandler<UploadPlantPhotoCommand, FileUploadResponse> handler) : ControllerBase
{
    [HttpPost("photos")]
    [ProducesResponseType<FileUploadResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadPhoto(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);

        var command = new UploadPlantPhotoCommand(
            currentUserAccessor.GetRequiredUserId(),
            file.FileName,
            file.ContentType,
            memoryStream.ToArray());

        await validator.ValidateAndThrowAsync(command, cancellationToken);
        var response = await handler.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(UploadPhoto), response);
    }
}
