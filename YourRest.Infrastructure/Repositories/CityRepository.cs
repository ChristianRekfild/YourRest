using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Domain.Repositories;

namespace YourRest.Infrastructure.Repositories
{
        public class CityRepository : PgRepository<City, int>, ICityRepository
    {
        public CityRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
