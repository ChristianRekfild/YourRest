using YourRest.Domain.Entities;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RegionRepository : PgRepository<Region, int>, IRegionRepository
    {
        public RegionRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
