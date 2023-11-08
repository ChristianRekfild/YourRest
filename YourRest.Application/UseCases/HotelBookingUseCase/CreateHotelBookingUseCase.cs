using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class CreateHotelBookingUseCase : ICreateHotelBookingUseCase
    { 
        private readonly IHotelBookingRepository _hotelBookingRepository;

        public CreateHotelBookingUseCase(IHotelBookingRepository hotelBookingRepository)
        {
            this._hotelBookingRepository = hotelBookingRepository;
        }

        public async Task<HotelBookingWithIdDto> ExecuteAsync(HotelBookingDto hotelBookingDto, CancellationToken token = default)
        {
            HotelBooking hotelBooking = new HotelBooking()
            {
                AccommodationId = hotelBookingDto.AccommodationId,
                DateFrom = hotelBookingDto.DateFrom,
                DateTo = hotelBookingDto.DateTo,
                RoomId = hotelBookingDto.RoomId,
                TotalAmount = hotelBookingDto.TotalAmount,
                AdultNr = hotelBookingDto.AdultNr,
                ChildrenNr = hotelBookingDto.ChildrenNr
            };
            var BookingAfterDate = await _hotelBookingRepository.
                GetAllWithIncludeAsync(t => t.DateFrom >= hotelBookingDto.DateFrom,
                token);
            var AlreadyHaveBooking = BookingAfterDate.Select(x => x).Where(t => t.DateTo <= hotelBookingDto.DateTo);
            if (AlreadyHaveBooking != null)
            {
                throw new Exception();
            }

            var savedHotelBooking = await _hotelBookingRepository.AddAsync(hotelBooking, true, token);

            HotelBookingWithIdDto hotelBookingWithIdDto = new HotelBookingWithIdDto()
            {
                Id = savedHotelBooking.Id,
                AccommodationId = savedHotelBooking.AccommodationId,
                DateFrom = savedHotelBooking.DateFrom,
                DateTo = savedHotelBooking.DateTo,
                RoomId = savedHotelBooking.RoomId,
                TotalAmount = savedHotelBooking.TotalAmount,
                AdultNr = savedHotelBooking.AdultNr,
                ChildrenNr = savedHotelBooking.ChildrenNr
            };

            return hotelBookingWithIdDto;
        }
    }
}
