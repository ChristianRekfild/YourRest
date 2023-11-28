using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
using YourRest.Domain.Models;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomRepository : PgRepository<Room, int>, IRoomRepository
    {
        public RoomRepository(SharedDbContext dataContext) : base(dataContext)
        {
        }
        public Task<List<Room>> GetRoomsByCityAndDatesAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken cancellation = default) 
        {
            return this._dataContext.Set<Room>()
                .Where(room => room.Accommodation.Address.CityId == cityId)
                .Where(room => !room.bookings.Any() || 
                !room.bookings.Contains(_dataContext.Set<Booking>()
                .Where(booking => booking.Rooms.Contains(room) && (booking.StartDate <= startDate && startDate < booking.EndDate) ||
                (booking.StartDate < endDate && endDate < booking.EndDate) ||
                (startDate <= booking.StartDate && booking.EndDate <= endDate)).FirstOrDefault())).ToListAsync(cancellation);


            throw new NotImplementedException();
        }

    }
}
