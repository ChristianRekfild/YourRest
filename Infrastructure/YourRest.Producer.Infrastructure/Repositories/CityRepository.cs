using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CityRepository : PgRepository<City, int>, ICityRepository
    {
        private readonly SharedDbContext _context;

        public CityRepository(SharedDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<City>> GetCitiesWithPhotosAsync(bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            IQueryable<City> citiesQuery = _context.Cities.Include(c => c.CityPhotos);

            if (isOnlyFavorite)
            {
                citiesQuery = citiesQuery.Where(c => c.IsFavorite);
            }

            return await citiesQuery.ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<City>> GetCitiesWithPhotosByRegionAsync(int regionId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            var citiesQuery = _context.Cities
                .Include(c => c.CityPhotos)
                .Where(c => c.RegionId == regionId);

            if (isOnlyFavorite)
            {
                citiesQuery = citiesQuery.Where(c => c.IsFavorite);
            }

            return await citiesQuery.ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<City>> GetCitiesWithPhotosByCountryAsync(int countryId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            var regionIds = await _context.Regions
                .Where(r => r.CountryId == countryId)
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            if (!regionIds.Any())
            {
                return new List<City>();
            }

            var citiesQuery = _context.Cities
                .Include(c => c.CityPhotos)
                .Where(c => regionIds.Contains(c.RegionId));

            if (isOnlyFavorite)
            {
                citiesQuery = citiesQuery.Where(c => c.IsFavorite);
            }

            return await citiesQuery.ToListAsync(cancellationToken);
        }
        
        public async Task<City> GetCityWithPhotosAsync(int id)
        {
            return await _context.Cities
                .Include(c => c.CityPhotos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
