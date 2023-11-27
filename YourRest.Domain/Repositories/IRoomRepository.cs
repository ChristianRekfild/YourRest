using YourRest.Domain.Entities;

namespace YourRest.Domain.Repositories
{
    public interface IRoomRepository : IRepository<Room, int>
    {
        Task<List<Room>> GetRoomsByCityAndDatesAsync(DateOnly start, DateOnly endDate, int cityId, CancellationToken cancellation = default);
    }
}