using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Repositories
{
    public class CityRepository : PgRepository<City, int>, ICityRepository
    {
        public CityRepository(SharedDbContext dataContext) : base(dataContext) { }

        public async Task<City> GetCityByIdAsync(int Id)
        {
            return await _dataContext.Cities.FindAsync(Id);
        }

        public async Task<IEnumerable<City>> GetCityListAsync()
        {
            return await _dataContext.Cities.ToListAsync();
        }

    }
}
