using FluentValidation;
using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Dto.Validators
{
    public class RoomFacilityDtoValidator: AbstractValidator<RoomFacilityDto>
    {
        public RoomFacilityDtoValidator()
        {
            RuleFor(roomFacility => roomFacility.Name).NotNull().NotEmpty().MaximumLength(50);
        }
    }
}
