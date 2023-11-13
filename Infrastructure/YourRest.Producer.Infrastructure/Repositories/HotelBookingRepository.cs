using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories;

public class HotelBookingRepository : PgRepository<HotelBooking, int>, IBookingRepository
{
    public HotelBookingRepository(SharedDbContext context) : base(context)
    {
    }
}