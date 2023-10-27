using FluentValidation;

namespace YourRest.Application.Dto.Validators
{
    public class AgeRangeWithIdDtoValidator : AbstractValidator<AgeRangeWithIdDto>
    {
        public AgeRangeWithIdDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(ageRange => ageRange.AgeFrom).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(ageRange => ageRange.AgeTo).NotNull().NotEmpty().GreaterThanOrEqualTo(ageRange => ageRange.AgeFrom);
        }
    }
}
