using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using YourRest.WebApi.BookingContext.Domain.Ports;
using YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories
{
    public class RegionRepository : PgRepository<Region, int>, IRegionRepository
    {
        public RegionRepository(SharedDbContext dataContext) : base(dataContext) { }

        public async Task<Region> GetRegionByIdAsync(int id)
        {
            return await _dataContext.Regions.FindAsync(id);
        }

        public async Task<IEnumerable<Region>> GetRegionListAsync()
        {
            return await _dataContext.Regions.ToListAsync();
        }
    }
}
