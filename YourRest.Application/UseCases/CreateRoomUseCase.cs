using YourRest.Application.Exceptions;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases
{
    public class CreateRoomUseCase : ICreateRoomUseCase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public CreateRoomUseCase(IRoomRepository roomRepository, IAccommodationRepository accommodationRepository)
        {
            _roomRepository = roomRepository;
            _accommodationRepository = accommodationRepository;
        }

        public async Task<RoomWithIdDto> Execute(RoomDto roomDto)
        {
            var accommodation = await _accommodationRepository.GetAsync(roomDto.AccommodationId);

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {roomDto.AccommodationId} not found");
            }

            var room = new RoomEntity();
            room.SquareInMeter = roomDto.SquareInMeter;
            room.Name = roomDto.Name;
            room.AccommodationId = accommodation.Id;
            room.Capacity = roomDto.Capacity;
            room.RoomType = roomDto.RoomType;

            var savedRoom = await _roomRepository.AddAsync(room);

            var savedRoomDto = new RoomWithIdDto
            {
                Id = savedRoom.Id,
                SquareInMeter = savedRoom.SquareInMeter,
                Name = savedRoom.Name,
                AccommodationId = savedRoom.AccommodationId,
                Capacity = savedRoom.Capacity,
                RoomType = savedRoom.RoomType
            };

            return savedRoomDto;
        }
    }
}