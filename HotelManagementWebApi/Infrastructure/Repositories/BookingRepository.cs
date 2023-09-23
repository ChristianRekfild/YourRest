using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using HotelManagementWebApi.Domain.Entities;
using YourRest.Infrastructure.Repositories;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class BookingRepository : PgRepository<Booking, int>, IBookingRepository
{
    public BookingRepository(HotelManagementDbContext context) : base(context)
    {
    }
}