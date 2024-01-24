using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IBookingRepository : IRepository<BookingDto, int>
    {
    }
}