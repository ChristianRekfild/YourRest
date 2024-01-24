using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface ICountryRepository : IRepository<CountryDto, int>
    {        
    }
}