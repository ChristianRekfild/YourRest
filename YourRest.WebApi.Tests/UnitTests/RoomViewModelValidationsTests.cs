﻿using FluentValidation.TestHelper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Validators;

namespace YourRest.WebApi.Tests.UnitTests
{
    public class RoomViewModelValidationsTests
    {
        private RoomDtoValidator roomValidator;
        public RoomViewModelValidationsTests() => roomValidator = new();

        [Fact]
        public void Should_have_error_when_Name_is_null_or_empty()
        {
            var model = CreateRoomViewModel();
            model.Name = null;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
            model.Name = string.Empty;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
        }
        [Fact]
        public void Should_have_error_when_NameLength_is_more_100_symbols()
        {
            var model = CreateRoomViewModel();
            model.Name = string.Join("", Enumerable.Range(1,101));
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
        }
        [Fact]
        public void Should_have_error_when_RoomType_is_null_or_empty()
        {
            var model = CreateRoomViewModel();
            model.RoomTypeId = 0;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomTypeId);
            model.Name = string.Empty;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomTypeId);
        }
        [Fact]
        public void Should_have_error_when_Capacity_is_less_or_equal_Zero()
        {
            var model = CreateRoomViewModel();
            model.Capacity = -1;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Capacity);
            model.Capacity = 0;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Capacity);

        }
        [Fact]
        public void Should_have_error_when_SquareInMeter_is_less_or_equal_Zero()
        {
            var model = CreateRoomViewModel();
            model.SquareInMeter = -10;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.SquareInMeter);
            model.SquareInMeter = 0;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.SquareInMeter);
        }

        [Fact]
        public void Should_have_error_when_All_fields_of_roomViewModel_is_not_valid()
        {
            var model = CreateRoomViewModel();
            model.Name = null;
            model.Capacity = 0;
            model.SquareInMeter = 0;
            model.RoomTypeId = 0;
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Name);
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.RoomTypeId);
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.Capacity);
            roomValidator.TestValidate(model).ShouldHaveValidationErrorFor(person => person.SquareInMeter);
        }
        private static RoomWithIdDto CreateRoomViewModel() => new RoomWithIdDto
        {
            Id = 0,
            Name = "305",
            Capacity = 10,
            SquareInMeter = 11,
            RoomTypeId = 3
        };
    }
}
