using HotelManagementWebApi.Domain.Entities.Booking;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    public BookingRepository(HotelManagementDbContext context) : base(context)
    {
    }
}