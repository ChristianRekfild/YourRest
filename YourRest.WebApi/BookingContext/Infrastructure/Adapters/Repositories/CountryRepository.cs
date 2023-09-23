using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories
{
    public class CountryRepository : PgRepository<Country, int>, ICountryRepository
    {
        public CountryRepository(SharedDbContext dataContext) : base(dataContext) { }
    }
}
