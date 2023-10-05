using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomFacilityRepository : PgRepository<RoomFacilityEntity, int>, IRoomFacilityRepository
    {
        public RoomFacilityRepository(SharedDbContext context) : base(context)
        {
        }
    }
}
