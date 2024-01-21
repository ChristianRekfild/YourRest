using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Infrastructure.Core.Contracts.Repositories;


namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class GetBookingDatesByRoomIdUseCase : IGetBookingDatesByRoomIdUseCase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;

        public GetBookingDatesByRoomIdUseCase(
            IBookingRepository bookingRepository,          
            IRoomRepository roomRepository
            )
        {
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;          
        }

        public async Task<List<RoomOccupiedDateDto>> ExecuteAsync(int RoomId, CancellationToken token = default)
        {
            DateOnly dateNow = DateOnly.FromDateTime(DateTime.Today);
            var roomsExist = await roomRepository.GetAsync(RoomId, token);

            if (roomsExist == null)
            {
                throw new InvalidParameterException("Бронируемой комнаты не существует.");
            }
 
            var bookingList = await bookingRepository.FindAsync(booking => booking.Rooms.Contains(roomsExist) && 
                booking.EndDate > dateNow, token);

            List<RoomOccupiedDateDto> OccupiedDates = new List<RoomOccupiedDateDto>();
            foreach ( var booking in bookingList ) 
            {
                RoomOccupiedDateDto tempOccupiedDate = new RoomOccupiedDateDto()
                {
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate
                };
                OccupiedDates.Add(tempOccupiedDate);
            }
            return OccupiedDates;
        }
    }
}

