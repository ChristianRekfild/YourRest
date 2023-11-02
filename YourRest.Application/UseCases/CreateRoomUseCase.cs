using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
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

        public async Task<RoomWithIdDto> Execute(RoomDto roomDto, int accommodationId, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetAsync(accommodationId, cancellationToken);

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {accommodationId} not found");
            }

            var room = new RoomEntity();
            room.SquareInMeter = roomDto.SquareInMeter;
            room.Name = roomDto.Name;
            room.AccommodationId = accommodation.Id;
            room.Capacity = roomDto.Capacity;
            room.RoomType = roomDto.RoomType;

            var savedRoom = await _roomRepository.AddAsync(room, cancellationToken:cancellationToken);

            var savedRoomDto = new RoomWithIdDto
            {
                Id = savedRoom.Id,
                SquareInMeter = savedRoom.SquareInMeter,
                Name = savedRoom.Name,
                Capacity = savedRoom.Capacity,
                RoomType = savedRoom.RoomType
            };

            return savedRoomDto;
        }
    }
}