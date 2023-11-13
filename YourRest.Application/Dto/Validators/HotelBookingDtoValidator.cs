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
    public class HotelBookingDtoValidator : AbstractValidator<BookingDto>
    {
        public HotelBookingDtoValidator() 
        {
           
            RuleFor(HotelBookingDto => HotelBookingDto.StartDate).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(HotelBookingDto => HotelBookingDto.EndDate).NotNull().NotEmpty().GreaterThan(HotelBookingDto => HotelBookingDto.StartDate);
            
            RuleForEach(HotelBookingDto => HotelBookingDto.Rooms).NotNull().NotEmpty(); // Можно проверить через SetValidator или Mast
            
            RuleFor(HotelBookingDto => HotelBookingDto.TotalAmount).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.AdultNr).NotNull().NotEmpty().GreaterThan(-1);
            RuleFor(HotelBookingDto => HotelBookingDto.ChildrenNr).NotNull().NotEmpty().GreaterThan(-1);
        }
    }
}
