using FluentValidation;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.Dto.Validators
{
    public class AddressDtoValidator: AbstractValidator<AddressWithIdDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(address => address.Street).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(address => address.ZipCode).NotNull().NotEmpty().Matches(@"^\d{6}$");
            RuleFor(address => address.Longitude).InclusiveBetween(-180,180);
            RuleFor(address => address.Latitude).InclusiveBetween(-90,90);
            RuleFor(address => address.CityId).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
