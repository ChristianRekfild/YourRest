using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using HotelManagementWebApi.Domain.Entities.Booking;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    private readonly HotelManagementDbContext _context;

    public BookingRepository(HotelManagementDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Booking> FindAsync(int id)
    {
        return await _context.Bookings.FindAsync(id);
    }
}