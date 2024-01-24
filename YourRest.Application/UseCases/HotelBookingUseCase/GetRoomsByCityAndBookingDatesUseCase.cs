using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Infrastructure.Core.Contracts.Repositories;


namespace YourRest.Application.UseCases.HotelBookingUseCase
{
    public class GetRoomsByCityAndBookingDatesUseCase : IGetRoomsByCityAndBookingDatesUseCase
    {
        private readonly IRoomRepository roomRepository;
        private readonly ICityRepository cityRepository;
        private readonly IMapper mapper;


        public GetRoomsByCityAndBookingDatesUseCase(
            IMapper mapper,
            IRoomRepository roomRepository,
            ICityRepository cityRepository
            )
        {
            this.roomRepository = roomRepository;
            this.cityRepository = cityRepository;
            this.mapper = mapper;
        }

        public async Task<List<RoomWithIdDto>> ExecuteAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken token = default)
        {
            
            var cityExist = await cityRepository.FindAnyAsync(t => t.Id == cityId);
            if (!cityExist)
            {
                throw new InvalidParameterException("Города с таким ID не существует.");
            }
            var resultRooms = await roomRepository.GetRoomsByCityAndDatesAsync(startDate, endDate, cityId, token);
            var resultRoomsDto = new List<RoomWithIdDto>();

            foreach (var room in resultRooms)
            {
                resultRoomsDto.Add(mapper.Map<RoomWithIdDto>(room));
            }

            return resultRoomsDto;
        }
    }
}

