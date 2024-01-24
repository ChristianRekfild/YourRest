using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RegionRepository : PgRepository<Region, int, RegionDto>, IRegionRepository
    {
        public RegionRepository(SharedDbContext dataContext, IMapper mapper) : base(dataContext, mapper) { }
    }
}
