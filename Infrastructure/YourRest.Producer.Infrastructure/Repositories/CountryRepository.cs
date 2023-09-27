using YourRest.Domain.Entities;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CountryRepository : PgRepository<Country, int>, ICountryRepository
    {
        public CountryRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
