using FluentValidation;

namespace AiGarden.AiService.UseCases.Commands;

public sealed class StartPlantAnalysisCommandValidator : AbstractValidator<StartPlantAnalysisCommand>
{
    public StartPlantAnalysisCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PhotoUrl)
            .NotEmpty()
            .Must(photoUrl => Uri.IsWellFormedUriString(photoUrl, UriKind.Absolute))
            .WithMessage("PhotoUrl must be a valid absolute url.");
        RuleFor(x => x.Provider).IsInEnum();
    }
}
