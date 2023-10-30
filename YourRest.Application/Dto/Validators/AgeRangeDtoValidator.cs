using FluentValidation;

namespace YourRest.Application.Dto.Validators
{
    public class AgeRangeDtoValidator : AbstractValidator<AgeRangeDto>
    {
        public AgeRangeDtoValidator()
        {
            RuleFor(ageRange => ageRange.AgeFrom).NotNull().GreaterThan(-1);
            RuleFor(ageRange => ageRange.AgeTo).NotNull().GreaterThanOrEqualTo(ageRange => ageRange.AgeFrom);
        }
    }
}
