using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
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
            if ((await roomRepository.GetWithIncludeAsync(room => room.Id == roomId, include => include.RoomFacilities)).SingleOrDefault() is not RoomEntity room)
            {
                throw new RoomNotFoundExeption(roomId);
            }
            if (!room.RoomFacilities.Any())
            {
                throw new RoomFacilityNotFoundException($"Not found RoomFacility in cuurent room (id : {roomId})");
            }
            return room.RoomFacilities.ToViewModel();
        }
    }
}
