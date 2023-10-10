using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.ValueObjects.Reviews;
using YourRest.Domain.Repositories;

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
            // ���� ��� ������� ����� ����������
            //var roomCheck = await _roomRepository.GetWithIncludeAsync(t => t.Name == roomDto.Name && t.AccommodationId == roomDto.AccommodationId);  

            //if (roomCheck.Any())
            //{
            //    throw new RoomCondlictException($"Room with name {roomDto.Name} already exist");
            //}

            var accommodation = await _accommodationRepository.GetAsync(roomDto.AccommodationId);

            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(roomDto.AccommodationId);
            }
            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(roomDto.AccommodationId);
            }


            var room = new Room();
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