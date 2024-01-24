using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Infrastructure.Core.Contracts.Repositories;


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
            var resultRooms = await roomRepository.GetRoomsByAccommodationAndDatesAsync(startDate, endDate, accommodationId, token);
            var resultRoomsDto = new List<RoomWithIdDto>();

            foreach (var room in resultRooms)
            {
                resultRoomsDto.Add(mapper.Map<RoomWithIdDto>(room));
            }

            return resultRoomsDto;
        }
    }
}

