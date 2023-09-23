using HotelManagementWebApi.Domain.Repositories;
using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    public BookingRepository(SharedDbContext context) : base(context)
    {
    }
}