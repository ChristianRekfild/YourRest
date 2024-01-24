using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IRoomRepository : IRepository<RoomDto, int>
    {
        Task<List<RoomDto>> GetRoomsByCityAndDatesAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken cancellation = default);
        Task<List<RoomDto>> GetRoomsByAccommodationAndDatesAsync(DateOnly startDate, DateOnly endDate, int accommodationId, CancellationToken cancellation = default);
    }
}