using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YourRest.Domain.Entities;
using YourRest.Domain.Models;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using YourRest.Producer.Infrastructure.Repositories.Extensions;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class AccommodationRepository : PgRepository<Accommodation, int>, IAccommodationRepository
    {
        private readonly SharedDbContext _context;

        public AccommodationRepository(SharedDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accommodation>> GetHotelsByFilter(AccommodationFilterCriteria filter, CancellationToken cancellationToken = default)
        {
            Expression<Func<Accommodation, bool>> filterExpression = h => true;

            if (filter.CountryIds != null && filter.CountryIds.Any())
            {
                filterExpression = filterExpression.And(h => h.Address != null && filter.CountryIds.Contains(h.Address.City.Region.CountryId));
            }

            if (filter.CityIds != null && filter.CityIds.Any())
            {
                filterExpression = filterExpression.And(h => h.Address != null && filter.CityIds.Contains(h.Address.CityId));
            }

            if (filter.AccommodationTypesIds != null && filter.AccommodationTypesIds.Any())
            {
                filterExpression = filterExpression.And(h => filter.AccommodationTypesIds.Contains(h.AccommodationTypeId));
            }

            return await GetWithIncludeAsync(filterExpression, cancellationToken,
                h => h.Address,
                h => h.StarRating,
                h => h.AccommodationType
            );
        }
        
        public async Task<IEnumerable<Accommodation>> GetAccommodationsWithFacilitiesAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Accommodations
                .Where(a => a.Id == id)
                .Include(a => a.AccommodationFacilities)
                .ThenInclude(af => af.AccommodationFacility)
                .ToListAsync(cancellationToken);
        }
    }
}