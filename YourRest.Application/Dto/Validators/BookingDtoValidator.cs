using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto.Models.HotelBooking;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourRest.Application.Dto.Validators
{
    public class BookingDtoValidator : AbstractValidator<BookingDto>
    {
        public BookingDtoValidator()
        {
            RuleFor(HotelBookingDto => HotelBookingDto.StartDate).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(HotelBookingDto => HotelBookingDto.EndDate).NotNull().NotEmpty().GreaterThan(HotelBookingDto => HotelBookingDto.StartDate);
            RuleForEach(HotelBookingDto => HotelBookingDto.Rooms).NotNull().NotEmpty(); // Можно проверить через SetValidator или Mast
            RuleFor(HotelBookingDto => HotelBookingDto.TotalAmount).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.AdultNumber).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.ChildrenNumber).NotNull().NotEmpty().GreaterThan(-1);
        }
    }
}
