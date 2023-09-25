using YourRest.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Domain.Repositories;

namespace YourRest.Infrastructure.Repositories
{
    public class RegionRepository : PgRepository<Region, int>, IRegionRepository
    {
        public RegionRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
