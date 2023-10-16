using FluentValidation;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.Dto.Validators
{
    public class RoomFacilityViewModelValidator: AbstractValidator<RoomFacilityViewModel>
    {
        public RoomFacilityViewModelValidator()
        {
            RuleFor(roomFacility => roomFacility.RoomId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(roomFacility => roomFacility.Name).NotNull().NotEmpty();
        }
    }
}
