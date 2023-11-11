using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Exceptions;
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
            HotelBooking hotelBookingToInsert = new HotelBooking()
            {
                AccommodationId = hotelBookingDto.AccommodationId,
                DateFrom = hotelBookingDto.DateFrom,
                DateTo = hotelBookingDto.DateTo,
                RoomId = hotelBookingDto.RoomId,
                TotalAmount = hotelBookingDto.TotalAmount,
                AdultNr = hotelBookingDto.AdultNr,
                ChildrenNr = hotelBookingDto.ChildrenNr
            };

            var RoomIdBooking = await _hotelBookingRepository.
                FindAsync(t => t.RoomId == hotelBookingDto.RoomId, token);

                //  Exception: не может выбрать ниже описанное
                //&&
                //t.DateFrom >= hotelBookingDto.DateFrom &&
                //t.DateFrom < hotelBookingDto.DateTo;
            var AlreadyHaveBooking = RoomIdBooking.Select(x => x)
                .Where(x => 
                (x.DateFrom  <= hotelBookingToInsert.DateFrom && hotelBookingToInsert.DateFrom <  x.DateTo) ||
                (x.DateFrom < hotelBookingToInsert.DateTo && hotelBookingToInsert.DateTo < x.DateTo)||
                (hotelBookingToInsert.DateFrom <= x.DateFrom && x.DateTo < hotelBookingToInsert.DateTo)
                ).ToList();

            if (AlreadyHaveBooking.Any())
            {
                throw new InvalidParameterException("Бронирование на выбранные даты невозможно. Комната занята.");
            }

            var savedHotelBooking = await _hotelBookingRepository.AddAsync(hotelBookingToInsert, true, token);

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
