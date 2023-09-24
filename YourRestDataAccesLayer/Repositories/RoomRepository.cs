using Microsoft.EntityFrameworkCore;
using YourRestDataAccesLayer.Abstractions;
using YourRestDomain.Entities;

namespace YourRestDataAccesLayer.Repositories
{
    public class RoomRepository : Repository<RoomEntity>, IRoomRepository
    {
        public RoomRepository(DbContext context) : base(context)
        {
        }
    }
}
