using FluentValidation;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.Dto.Validators
{
    public class RoomViewModelValidator : AbstractValidator<RoomViewModel>
    {
        public RoomViewModelValidator()
        {
            RuleFor(room => room.AccommodationId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(room => room.Name).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(room => room.SquareInMeter).GreaterThan(0);
            RuleFor(room => room.RoomType).NotNull().NotEmpty();
            RuleFor(room => room.Capacity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
