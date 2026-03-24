using FluentValidation;

namespace AiGarden.StorageS3Service.UseCases.Commands;

public sealed class UploadPlantPhotoCommandValidator : AbstractValidator<UploadPlantPhotoCommand>
{
    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];

    public UploadPlantPhotoCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(AllowedContentTypes.Contains)
            .WithMessage("Only jpeg, png and webp images are supported.");
        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(content => content.Length <= 10 * 1024 * 1024)
            .WithMessage("Maximum file size is 10 MB.");
    }
}
