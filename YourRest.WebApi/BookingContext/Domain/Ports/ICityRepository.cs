using SharedKernel.Domain.Entities;

namespace YourRest.WebApi.BookingContext.Domain.Ports
{
    public interface ICityRepository
    {
        Task<City> GetCityByIdAsync(int Id);

        Task<IEnumerable<City>> GetCityListAsync();

    }
}
