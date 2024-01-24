using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class CreateRoomUseCase : ICreateRoomUseCase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public CreateRoomUseCase(
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IAccommodationRepository accommodationRepository
        )
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _accommodationRepository = accommodationRepository;
        }

        public async Task<RoomWithIdDto> ExecuteAsync(RoomDto roomDto, int accommodationId, CancellationToken cancellationToken)
        {
            var roomType = await _roomTypeRepository.GetAsync(roomDto.RoomTypeId, cancellationToken);

            if (roomType == null)
            {
                throw new EntityNotFoundException($"Room Type with id {roomDto.RoomTypeId} not found");
            }

            var accommodation = await _accommodationRepository.GetAsync(accommodationId, cancellationToken);

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {accommodationId} not found");
            }

            var room = new Infrastructure.Core.Contracts.Models.RoomDto
            {
                SquareInMeter = roomDto.SquareInMeter,
                Name = roomDto.Name,
                AccommodationId = accommodation.Id,
                Accommodation = accommodation,
                Capacity = roomDto.Capacity,
                RoomTypeId = roomType.Id,
                RoomType = roomType
            };
            var savedRoom = await _roomRepository.AddAsync(room, cancellationToken: cancellationToken);

            var savedRoomDto = new RoomWithIdDto
            {
                Id = savedRoom.Id,
                SquareInMeter = savedRoom.SquareInMeter,
                Name = savedRoom.Name,
                Capacity = savedRoom.Capacity,
                RoomTypeId = savedRoom.RoomTypeId
            };

            return savedRoomDto;
        }
    }
}