using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Facility
{
    public class GetRoomFacilitiesByRoomIdUseCase : IGetRoomFacilitiesByRoomIdUseCase
    {
        private readonly IRoomRepository roomRepository;

        public GetRoomFacilitiesByRoomIdUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public Task<IEnumerable<RoomFacilityViewModel>> ExecuteAsync(int roomId)
        {
            if (roomRepository.GetWithIncludeAsync(room => room.Id == roomId, include => include.RoomFacilities).Result.FirstOrDefault() is not RoomEntity room)
            {
                throw new RoomNotFoundExeption(roomId);
            }
            return Task.FromResult(room.RoomFacilities.ToList().ToViewModel());
        }
    }
}
