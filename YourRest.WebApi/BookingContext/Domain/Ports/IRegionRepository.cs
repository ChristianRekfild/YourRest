using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourRest.WebApi.BookingContext.Domain.Ports
{
    public interface IRegionRepository
    {
        Task<Region> GetRegionByIdAsync(int id);

        Task<IEnumerable<Region>> GetRegionListAsync();
    }
}