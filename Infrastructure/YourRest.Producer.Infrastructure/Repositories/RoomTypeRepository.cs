using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Infrastructure.Repositories;

public class RoomTypeRepository : PgRepository<RoomType, int>, IRoomTypeRepository
{
    public RoomTypeRepository(SharedDbContext dataContext) : base(dataContext)
    {
    }
}