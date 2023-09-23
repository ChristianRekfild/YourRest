using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories
{
    public class RegionRepository : PgRepository<Region, int>, IRegionRepository
    {
        public RegionRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
