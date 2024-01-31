using YourRest.Domain.Entities;

namespace YourRest.Domain.Repositories
{
    public interface ICityRepository : IRepository<City, int>
    {
        Task<IEnumerable<City>> GetCitiesWithPhotosAsync(bool isOnlyFavorite,
            CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetCitiesWithPhotosByRegionAsync(int regionId, bool isOnlyFavorite,
            CancellationToken cancellationToken);

        Task<IEnumerable<City>> GetCitiesWithPhotosByCountryAsync(int countryId, bool isOnlyFavorite,
            CancellationToken cancellationToken);

        Task<City> GetCityWithPhotosAsync(int id);
    }
}
