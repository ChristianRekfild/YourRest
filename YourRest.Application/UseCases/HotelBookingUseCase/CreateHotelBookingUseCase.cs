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
        private readonly IBookingRepository _bookingRepository;

        public CreateHotelBookingUseCase(IBookingRepository hotelBookingRepository)
        {
            this._bookingRepository = hotelBookingRepository;
        }

        public async Task<HotelBookingWithIdDto> ExecuteAsync(BookingDto bookingDto, CancellationToken token = default)
        {
            Booking hotelBookingToInsert = new Booking()
            {
                AccommodationId = bookingDto.AccommodationId,
                DateFrom = bookingDto.DateFrom,
                DateTo = bookingDto.DateTo,
                RoomId = bookingDto.RoomId,
                TotalAmount = bookingDto.TotalAmount,
                AdultNr = bookingDto.AdultNr,
                ChildrenNr = bookingDto.ChildrenNr
            };

            var RoomIdBooking = await _bookingRepository.
                FindAsync(t => t.RoomId == bookingDto.RoomId, token);

                //  Exception: не может выбрать ниже описанное
                //&&
                //t.DateFrom >= bookingDto.DateFrom &&
                //t.DateFrom < bookingDto.DateTo;
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

            var savedHotelBooking = await _bookingRepository.AddAsync(hotelBookingToInsert, true, token);

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
