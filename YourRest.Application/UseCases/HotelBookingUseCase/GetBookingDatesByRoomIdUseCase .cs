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


namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class GetBookingDatesByRoomIdUseCase : IGetBookingDatesByRoomIdUseCase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public GetBookingDatesByRoomIdUseCase(
            IBookingRepository bookingRepository,
            IMapper mapper,
            ICustomerRepository customerRepository,
            IRoomRepository roomRepository
            )
        {
            this.bookingRepository = bookingRepository;
            this.mapper = mapper;
            this.roomRepository = roomRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<List<RoomOccupiedDateDto>> ExecuteAsync(int RoomId, CancellationToken token = default)
        {

            var roomsExist = await roomRepository.FindAsync(t => t.Id == RoomId);

            if (!roomsExist.Any())
            {
                throw new InvalidParameterException("Бронируемой комнаты не существует.");
            }

            var bookingList =  await bookingRepository.FindAsync(booking =>booking.Rooms.Contains((Domain.Entities.Room)roomsExist));

            List<RoomOccupiedDateDto> OccupiedDates = new List<RoomOccupiedDateDto>();
            foreach ( var booking in bookingList ) 
            {
                RoomOccupiedDateDto tempOccupiedDate = new RoomOccupiedDateDto()
                {
                    StartDate = DateOnly.FromDateTime(booking.StartDate),
                    EndDate = DateOnly.FromDateTime(booking.EndDate)
                };
            }
            return OccupiedDates;
        }
    }
}

