using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
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
            var roomsByCityId = this._dataContext.Set<Room>()
               .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Address.CityId == cityId);

            var roomsByCityIdInBooking = this._dataContext.Set<Room>()
                .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Address.CityId == cityId &&
                room.bookings.Any(b => (b.StartDate <= startDate && startDate < b.EndDate) ||
                (b.StartDate < endDate && endDate < b.EndDate) ||
                (startDate <= b.StartDate && b.EndDate <= endDate)));

            return roomsByCityId
                .Except(roomsByCityIdInBooking)
                .AsQueryable()
                .ToListAsync(cancellation);
        }
    }
}
