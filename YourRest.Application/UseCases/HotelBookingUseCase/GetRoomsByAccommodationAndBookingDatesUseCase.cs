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
    public class GetRoomsByAccommodationAndBookingDatesUseCase : IGetRoomsByAccommodationAndBookingDatesUseCase
    {
        private readonly IRoomRepository roomRepository;
        private readonly IAccommodationRepository accommodationRepository;
        private readonly IMapper mapper;


        public GetRoomsByAccommodationAndBookingDatesUseCase(
            IMapper mapper,
            IRoomRepository roomRepository,
            IAccommodationRepository accommodationRepository
            )
        {
            this.roomRepository = roomRepository;
            this.accommodationRepository = accommodationRepository;
            this.mapper = mapper;
    }

        public async Task<List<RoomWithIdDto>> ExecuteAsync(DateOnly startDate, DateOnly endDate, int accommodationId, CancellationToken token = default)
        {
            
            var cityExist = await accommodationRepository.FindAnyAsync(t => t.Id == accommodationId);
            if (!cityExist)
            {
                throw new InvalidParameterException("Отеля с таким ID не существует.");
            }
            var resultRooms = await roomRepository.GetRoomsByCityAndDatesAsync(startDate, endDate, accommodationId, token);
            var resultRoomsDto = new List<RoomWithIdDto>();

            foreach (var room in resultRooms)
            {
                resultRoomsDto.Add(mapper.Map<RoomWithIdDto>(room));
            }

            return resultRoomsDto;
        }
    }
}

