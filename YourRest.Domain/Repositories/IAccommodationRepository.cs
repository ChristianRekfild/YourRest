using YourRest.Domain.Entities;
using YourRest.Domain.Models;
using YourRest.Domain.ValueObjects;

namespace YourRest.Domain.Repositories;
public interface IAccommodationRepository : IRepository<Accommodation, int>
{
    Task<IEnumerable<Accommodation>> GetHotelsByFilter(int userId, AccommodationFilterCriteria filter,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Accommodation>> GetAccommodationsWithFacilitiesAsync(int id,
        CancellationToken cancellationToken = default);

    Task UpdateStateAsync(int messageId, AccommodationState state);
}