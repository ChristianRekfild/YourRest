using YourRest.Application.CustomErrors;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Room
{
    public class RemoveRoomUseCase : IRemoveRoomUseCase
    {
        private readonly IRoomRepository roomRepository;
        public RemoveRoomUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(int id)
        {
            if (await roomRepository.GetAsync(id) is not RoomEntity room)
            {
                throw new RoomNotFoundExeption(id);
            }
            await roomRepository.DeleteAsync(id);
        }
    }
}
