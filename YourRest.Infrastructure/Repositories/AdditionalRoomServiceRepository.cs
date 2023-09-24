using YourRest.Infrastructure.DbContexts;
using YourRestDomain.Entities;

namespace YourRest.Infrastructure.Repositories
{
    public class AdditionalRoomServiceRepository : Repository<AdditionalRoomServiceEntity>
    {
        public AdditionalRoomServiceRepository(SharedDbContext context) : base(context)
        {
        }
    }
}
