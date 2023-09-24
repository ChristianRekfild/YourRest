using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourRest.WebApi.BookingContext.Domain.Ports
{
    public interface ICountryRepository
    {
        Task<CountryEntity> GetCountryByIdAsync(int id);

        Task<IEnumerable<CountryEntity>> GetCountryListAsync();
    }
}