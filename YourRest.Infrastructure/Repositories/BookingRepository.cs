using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    public BookingRepository(SharedDbContext context) : base(context)
    {
    }
}