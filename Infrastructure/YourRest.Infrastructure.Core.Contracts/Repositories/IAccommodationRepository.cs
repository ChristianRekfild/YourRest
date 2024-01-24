using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IAccommodationRepository : IRepository<AccommodationDto, int>
    {
        Task<IEnumerable<AccommodationDto>> GetHotelsByFilter(int userId, AccommodationFilterCriteriaDto filter,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<AccommodationDto>> GetAccommodationsWithFacilitiesAsync(int id,
            CancellationToken cancellationToken = default);
    }
}