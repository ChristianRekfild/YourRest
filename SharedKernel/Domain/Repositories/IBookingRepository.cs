using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

namespace SharedKernel.Domain.Repositories;
public interface IBookingRepository : IRepository<Booking, int>
{
}