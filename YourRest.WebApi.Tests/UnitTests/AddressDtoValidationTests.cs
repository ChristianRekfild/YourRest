using FluentValidation.TestHelper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Validators;

namespace YourRest.WebApi.Tests.UnitTests
{
    public class AddressDtoValidationTests
    {
        private readonly AddressDtoValidator addressValidator;
        public AddressDtoValidationTests()
        {
            addressValidator = new();
        }
        [Fact]
        public void Should_have_error_when_All_fields_of_addressDto_is_not_valid()
        {
            var model = CreateAddressDto();
            model.CityId = -1;
            model.ZipCode = string.Empty;
            model.Longitude = 190;
            model.Latitude = 190;
            model.Street = string.Empty;
            addressValidator.TestValidate(model).ShouldHaveValidationErrorFor(address => address.CityId);
            addressValidator.TestValidate(model).ShouldHaveValidationErrorFor(address => address.ZipCode);
            addressValidator.TestValidate(model).ShouldHaveValidationErrorFor(address => address.Longitude);
            addressValidator.TestValidate(model).ShouldHaveValidationErrorFor(address => address.Latitude);
            addressValidator.TestValidate(model).ShouldHaveValidationErrorFor(address => address.Street);
        }

        private AddressDto CreateAddressDto() 
        {
            return new AddressDto()
            {
                CityId = 1,
                ZipCode = "123456",
                Longitude = 170,
                Latitude = 81,
                Street = "Prospect Mira"
            };
        }
    }
}
