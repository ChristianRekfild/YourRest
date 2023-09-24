using SharedKernel.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourRest.WebApi.BookingContext.Domain.Ports
{
    public interface IRegionRepository
    {
        Task<RegionEntity> GetRegionByIdAsync(int id);

        Task<IEnumerable<RegionEntity>> GetRegionListAsync();
    }
}