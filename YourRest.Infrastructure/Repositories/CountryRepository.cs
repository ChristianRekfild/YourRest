using YourRest.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Domain.Repositories;

namespace YourRest.Infrastructure.Repositories
{
    public class CountryRepository : PgRepository<Country, int>, ICountryRepository
    {
        public CountryRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
