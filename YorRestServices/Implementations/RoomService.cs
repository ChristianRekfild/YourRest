using HotelManagementWebApi.ViewModels;
using YorRestDataTransferObject;
using YorRestServices.Abstracts;
using YourRestDataAccesLayer.Abstractions;

namespace YorRestServices.Implementations
{
    public class RoomService: IRoomService
    {
        private readonly IUnitOfWork unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddAdditionalRoomServiceAsync(AdditionalRoomServiceViewModel source)
        {

            var currentRoom = unitOfWork.RoomRepository.GetWithIncludeAsync(room => room.Id == source.RoomId, include => include.RoomServices)
                .Result.FirstOrDefault();
            
            if (currentRoom == null) throw new Exception("404 room not found");
            if (currentRoom.RoomServices.Contains(source.ToEntity())) throw new Exception($"422 unprocessible entity of {currentRoom.Name} room");
            await unitOfWork.AdditionalRoomServiceRepository.CreateAsync(source.ToEntity());
            await unitOfWork.SaveChangesAsync();
            
        }
    }
}
