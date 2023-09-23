using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

namespace YourRest.WebApi.BookingContext.Domain.Ports
{
    public interface IRegionRepository : IRepository<Region, int>
    {      
    }
}