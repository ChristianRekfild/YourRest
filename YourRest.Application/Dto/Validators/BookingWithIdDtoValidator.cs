using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto.Models.HotelBooking;

namespace YourRest.Application.Dto.Validators
{
    public class BookingWithIdDtoValidator : AbstractValidator<BookingWithIdDto>
    {
        public BookingWithIdDtoValidator()
        {
            RuleFor(HotelBookingDto => HotelBookingDto.Id).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(HotelBookingDto => HotelBookingDto).SetValidator(new BookingDtoValidator());
        }
    }
}
