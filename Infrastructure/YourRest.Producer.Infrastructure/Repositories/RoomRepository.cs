using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomRepository : PgRepository<Room, int>, IRoomRepository
    {
        public RoomRepository(SharedDbContext context) : base(context)
        {
        }
    }
}
