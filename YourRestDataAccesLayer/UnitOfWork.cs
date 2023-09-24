using Microsoft.EntityFrameworkCore;
using YourRestDataAccesLayer.Abstractions;
using YourRestDataAccesLayer.Repositories;

namespace YourRestDataAccesLayer
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbContext context;
        private IAdditionalRoomServiceRepository additionalRoomServiceRepository;
        private IRoomRepository roomRepository;
        public UnitOfWork(DbContext context)
        {

            this.context = context;

        }

        public IAdditionalRoomServiceRepository AdditionalRoomServiceRepository => additionalRoomServiceRepository ??= new AdditionalRoomServiceRepository(context);
        public IRoomRepository RoomRepository => roomRepository ??= new RoomRepository(context);

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
