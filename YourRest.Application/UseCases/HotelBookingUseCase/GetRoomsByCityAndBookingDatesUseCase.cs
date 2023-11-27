using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Domain.ValueObjects.Bookings;


namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class GetRoomsByCityAndBookingDatesUseCase : IGetRoomsByCityAndBookingDatesUseCase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly ICityRepository cityRepository;
        private readonly IAccommodationRepository accommodationRepository;
        private readonly IMapper mapper;


        public GetRoomsByCityAndBookingDatesUseCase(
            IBookingRepository bookingRepository,
            IMapper mapper,
            IRoomRepository roomRepository,
            ICityRepository cityRepository,
            IAccommodationRepository accommodationRepository
            )
        {
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;
            this.cityRepository = cityRepository;
            this.accommodationRepository = accommodationRepository;
            this.mapper = mapper;
    }

        public async Task<List<RoomDto>> ExecuteAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken token = default)
        {
            
            var cityExist = await cityRepository.FindAnyAsync(t => t.Id == cityId);
            if (!cityExist)
            {
                throw new InvalidParameterException("Города с таким ID не существует.");
            }
            var accomodationList = await accommodationRepository.GetAllWithIncludeAsync(accom => accom.Address.CityId == cityId);
            List<int> accomodationIdList = accomodationList.Select(accom => accom.Id).ToList();
            var roomsList = await roomRepository.FindAsync(room => accomodationIdList.Contains(room.AccommodationId));
            List<int> roomsListIdList = roomsList.Select(room => room.Id).ToList();

            var bookingList = await bookingRepository.GetAllWithIncludeAsync(booking =>
            ((booking.StartDate <= startDate && startDate < booking.EndDate) ||
            (booking.StartDate < endDate && endDate < booking.EndDate) ||
            (startDate <= booking.StartDate && booking.EndDate <= endDate))
            , token);

            foreach (var booking in bookingList)
            {
                roomsListIdList.Except(booking.Rooms.Select(r => r.Id));
            }

            var resultRooms = roomsList.Where(room => roomsListIdList.Contains(room.Id)).ToList();
            var resultRoomsDto = new List<RoomDto>();

            foreach (var room in resultRooms)
            {
                resultRoomsDto.Add(mapper.Map<RoomExtendedDto>(room));
            }

            return resultRoomsDto;
        }
    }
}

