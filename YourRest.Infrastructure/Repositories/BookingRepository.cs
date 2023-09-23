using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    public BookingRepository(SharedDbContext context) : base(context)
    {
    }
}