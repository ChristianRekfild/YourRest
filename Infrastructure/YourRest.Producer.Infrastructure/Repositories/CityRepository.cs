using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CityRepository : PgRepository<City, int>, ICityRepository
    {
        public CityRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
