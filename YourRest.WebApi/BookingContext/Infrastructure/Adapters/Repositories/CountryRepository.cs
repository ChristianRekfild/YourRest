using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using YourRest.WebApi.BookingContext.Domain.Ports;
using YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories
{
    public class CountryRepository : PgRepository<Country, int>, ICountryRepository
    {
        public CountryRepository(SharedDbContext dataContext) : base(dataContext) { }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            return await _dataContext.Countries.FindAsync(id);
        }

        public async Task<IEnumerable<Country>> GetCountryListAsync()
        {
            return await _dataContext.Countries.ToListAsync();
        }
    }
}
