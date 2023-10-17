using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Facility
{
    public class GetRoomFacilitiesByRoomIdUseCase : IGetFacilitiesByRoomIdUseCase
    {
        private readonly IRoomRepository roomRepository;
        public GetRoomFacilitiesByRoomIdUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomFacilityViewModel>> ExecuteAsync(int roomId)
        {
            var room = (await roomRepository.GetWithIncludeAsync(room => room.Id == roomId, include => include.RoomFacilities)).FirstOrDefault();
            if (room == null)
            {
                throw new EntityNotFoundException($"Room with id number {roomId} not found");
            }
            if (!room.RoomFacilities.Any())
            {
                throw new EntityNotFoundException($"Not found RoomFacility in current room (id : {roomId})");
            }
            return room.RoomFacilities.ToViewModel();
        }
    }
}
