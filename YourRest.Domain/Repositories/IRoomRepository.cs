using YourRest.Domain.Entities;

namespace YourRest.Domain.Repositories
{
    public interface IRoomRepository : IRepository<Room, int>
    {
        Task<List<Room>> GetRoomsByCityAndDatesAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken cancellation = default);
    }
}