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

        public async Task<RoomDto> Execute(RoomDto roomDto)
        {
            var room = await _roomRepository.GetAsync(roomDto.Id);
            var accomodation = await _accommodationRepository.GetAsync(roomDto.AccomodationId);

            if (room != null)
            {
                throw new RoomNotFoundException($"Room with Id {roomDto.Id} already exist");
            }

            if (accomodation == null)
            {
                throw new AccommodationNotFoundException(roomDto.AccomodationId);
            }

            Room savedRoom = new Room();
            savedRoom.Id = roomDto.Id;
            savedRoom.SquareInMeter = roomDto.SquareInMeter;
            savedRoom.Name = roomDto.Name;
            savedRoom.AccommodationId = accomodation.Id;
            savedRoom.Capacity = roomDto.Capacity;
            savedRoom.RoomType = roomDto.RoomType;


            await _roomRepository.AddAsync(savedRoom, false);
            await _roomRepository.SaveChangesAsync();

            return roomDto;
        }
    }
}