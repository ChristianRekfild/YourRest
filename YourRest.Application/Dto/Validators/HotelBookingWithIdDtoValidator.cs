using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto.Models.HotelBooking;

namespace YourRest.Application.Dto.Validators
{
    public class HotelBookingWithIdDtoValidator : AbstractValidator<BookingWithIdDto>
    {
        public HotelBookingWithIdDtoValidator()
        {
            RuleFor(HotelBookingDto => HotelBookingDto.Id).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(HotelBookingDto => HotelBookingDto).SetValidator(new HotelBookingDtoValidator());

            //RuleFor(HotelBookingDto => HotelBookingDto.AccommodationId).NotNull().NotEmpty().GreaterThan(0);                                        ЭТО ВСЕ НЕ НАДО ДУБЛИРОВАТЬ
            //RuleFor(HotelBookingDto => HotelBookingDto.DateFrom).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Today);                         
            //RuleFor(HotelBookingDto => HotelBookingDto.DateTo).NotNull().NotEmpty().GreaterThan(HotelBookingDto => HotelBookingDto.DateFrom);       
            //RuleFor(HotelBookingDto => HotelBookingDto.RoomId).NotNull().NotEmpty().GreaterThan(0);                                                 УДАЛИТЬ
            //RuleFor(HotelBookingDto => HotelBookingDto.TotalAmount).NotNull().NotEmpty().GreaterThan(-1);
            //RuleFor(HotelBookingDto => HotelBookingDto.AdultNr).NotNull().NotEmpty().GreaterThan(-1);
            //RuleFor(HotelBookingDto => HotelBookingDto.ChildrenNr).NotNull().NotEmpty().GreaterThan(-1);                                            УДАЛИТЬ 
        }
    }
}
