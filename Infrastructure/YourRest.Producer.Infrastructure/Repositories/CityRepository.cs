using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CityRepository : PgRepository<City, int, CityDto>, ICityRepository
    {
        public CityRepository(SharedDbContext dataContext, IMapper mapper) : base(dataContext, mapper) { }
    }
}
