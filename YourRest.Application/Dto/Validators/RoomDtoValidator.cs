using FluentValidation;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto.Validators
{
    public class RoomDtoValidator : AbstractValidator<RoomDto>
    {
        public RoomDtoValidator()
        {
            RuleFor(room => room.Name).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(room => room.SquareInMeter).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(room => room.RoomType).NotNull().NotEmpty();
            RuleFor(room => room.Capacity).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
