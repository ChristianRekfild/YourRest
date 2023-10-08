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

        public async Task<SavedRoomDto> Execute(SavedRoomDto roomDto)
        {
            var room = await _roomRepository.GetWithIncludeAsync(t => t.Name == roomDto.Name && t.AccommodationId == roomDto.AccommodationId);
            var accommodation = await _accommodationRepository.GetAsync(roomDto.AccommodationId);

            if (room.Count() != 0)
            {
                throw new RoomCondlictException($"Room with name {roomDto.Name} already exist");
            }


            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(roomDto.AccommodationId);
            }

            Room savedRoom = new Room();
            //savedRoom.Id = roomDto.Id;
            savedRoom.SquareInMeter = roomDto.SquareInMeter;
            savedRoom.Name = roomDto.Name;
            savedRoom.AccommodationId = accommodation.Id;
            savedRoom.Capacity = roomDto.Capacity;
            savedRoom.RoomType = roomDto.RoomType;


            await _roomRepository.AddAsync(savedRoom, false);
            await _roomRepository.SaveChangesAsync();

            return roomDto;
        }
    }
}