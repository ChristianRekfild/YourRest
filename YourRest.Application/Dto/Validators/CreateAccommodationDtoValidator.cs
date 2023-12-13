using FluentValidation;

namespace YourRest.Application.Dto.Validators;

public class CreateAccommodationDtoValidator : AbstractValidator<CreateAccommodationDto>
{
    public CreateAccommodationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.AccommodationTypeId)
            .NotEmpty()
            .WithMessage("Accommodation Type is required.");

        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5)
            .When(x => x.Stars != 0)
            .WithMessage("Stars must be between 1 and 5.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description is optional but must be a non-empty string if provided.");
    }
}

