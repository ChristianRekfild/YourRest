using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto.Models.HotelBooking;

namespace YourRest.Application.Dto.Validators
{
    public class HotelBookingWithIdDtoValidator : AbstractValidator<HotelBookingWithIdDto>
    {
        public HotelBookingWithIdDtoValidator()
        {
            RuleFor(HotelBookingDto => HotelBookingDto.Id).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(HotelBookingDto => HotelBookingDto.AccommodationId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(HotelBookingDto => HotelBookingDto.DateTo).NotNull().NotEmpty().GreaterThan(DateTime.Now);
            RuleFor(HotelBookingDto => HotelBookingDto.DateFrom).NotNull().NotEmpty().GreaterThan(DateTime.Now);
            RuleFor(HotelBookingDto => HotelBookingDto.RoomId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(HotelBookingDto => HotelBookingDto.TotalAmount).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.AdultNr).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.ChildrenNr).NotNull().NotEmpty().GreaterThan(-1);
        }
    }
}
