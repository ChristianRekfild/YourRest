using System.Threading;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Room
{
    public class RemoveRoomUseCase : IRemoveRoomUseCase
    {
        private readonly IRoomRepository roomRepository;
        public RemoveRoomUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetAsync(id, cancellationToken: cancellationToken);
            if(room == null)
            {
                throw new EntityNotFoundException($"Room with Id:{id} not found");
            }
            await roomRepository.DeleteAsync(id, cancellationToken: cancellationToken);
        }
    }
}
