using FluentValidation.TestHelper;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Dto.Validators;

namespace YourRest.WebApi.Tests.UnitTests
{
    public class RoomFacilityViewModelValidationsTests
    {
        private readonly RoomFacilityDtoValidator roomFacilityValidator;
        public RoomFacilityViewModelValidationsTests() => roomFacilityValidator = new();

        [Fact]
        public void Should_have_error_when_All_fields_of_roomFacilityViewModel_is_not_valid()
        {
            var model = CreateRoomFacilityViewModel();
            model.Name = null;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
        }

        [Fact]
        public void Should_have_error_when_Name_is_null_or_empty()
        {
            var model = CreateRoomFacilityViewModel();
            model.Name = null;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
            model.Name = string.Empty;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
        }
        [Fact]
        public void Should_have_error_when_NameLength_is_more_50_symbols()
        {
            var model = CreateRoomFacilityViewModel();
            model.Name = string.Join("", Enumerable.Range(1, 51));
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
        }

        private RoomFacilityDto CreateRoomFacilityViewModel() => new()
        {
            Name = "Air Conditioner"
        };

    }
}
