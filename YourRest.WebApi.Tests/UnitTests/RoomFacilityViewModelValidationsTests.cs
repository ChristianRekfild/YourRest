using FluentValidation.TestHelper;
using YourRest.Application.Dto.Models;
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
            model.RoomId = -1;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomId);
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
        [Fact]
        public void Should_have_error_when_RoomId_is_less_or_equal_Zero()
        {
            var model = CreateRoomFacilityViewModel();
            model.RoomId = -1;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomId);
            model.RoomId = 0;
            roomFacilityValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomId);
        }

        private RoomFacilityDto CreateRoomFacilityViewModel() => new()
        {
            Id = 0,
            RoomId = 1,
            Name = "Air Conditioner"
        };

    }
}
