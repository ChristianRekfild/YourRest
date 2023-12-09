using System.Linq;
using System.Linq.Expressions;
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
        public AccommodationRepository(SharedDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Accommodation>> GetHotelsByFilter(AccommodationFilterCriteria filter, CancellationToken cancellationToken = default)
        {
            Expression<Func<Accommodation, bool>> filterExpression = h => true;

            if (filter.CountryIds != null && filter.CountryIds.Any())
            {
                filterExpression = filterExpression.And(h => filter.CountryIds.Contains(h.Address.City.Region.CountryId));
            }

            if (filter.CityIds != null && filter.CityIds.Any())
            {
                filterExpression = filterExpression.And(h => filter.CityIds.Contains(h.Address.CityId));
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
    }
}