using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;

namespace HotelManagementWebApi.Domain.Repositories;
public interface IBookingRepository : IRepository<Booking, int>
{
}