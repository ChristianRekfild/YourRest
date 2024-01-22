using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.Producer.Infrastructure.Repositories.Extensions;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class AccommodationRepository : PgRepository<Accommodation, int, AccommodationDto>, IAccommodationRepository
    {
        private readonly SharedDbContext _context;
        private readonly IMapper _mapper;

        public AccommodationRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<AccommodationDto>> GetHotelsByFilter(int userId, AccommodationFilterCriteriaDto filter, CancellationToken cancellationToken = default)
        {
            Expression<Func<AccommodationDto, bool>> filterExpression = h => true;

            if (userId > 0)
            {
                filterExpression = filterExpression.And(h => h.UserAccommodations.Any(ua => ua.UserId == userId));
            }

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

            return _mapper.Map<IEnumerable<AccommodationDto>>(await GetWithIncludeAndTrackingAsync(filterExpression, cancellationToken,
                h => h.Address,
                h => h.StarRating,
                h => h.AccommodationType,
                h => h.UserAccommodations
            ));
        }
                
        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsWithFacilitiesAsync(int id, CancellationToken cancellationToken = default)
        {
            return _mapper.Map<IEnumerable<AccommodationDto>>(await _context.Accommodations
                .Where(a => a.Id == id)
                .Include(a => a.AccommodationFacilities)
                .ThenInclude(af => af.AccommodationFacility)
                .ToListAsync(cancellationToken));
        }

        #region AddAsync
        protected override IReadOnlyDictionary<string, object> DetachLinkedEntityAsync(Accommodation entity)
        {
            Dictionary<string, object> linkedEntity = new();

            SaveLinkedEntityCollection(entity.AccommodationFacilities, "AccommodationFacilities", linkedEntity);

            SaveLinkedEntityProperty(entity.AccommodationType, "AccommodationType", linkedEntity);

            SaveLinkedEntityProperty(entity.Address, "Address", linkedEntity);

            SaveLinkedEntityCollection(entity.Rooms, "Rooms", linkedEntity);

            SaveLinkedEntityProperty(entity.StarRating, "StarRating", linkedEntity);

            SaveLinkedEntityCollection(entity.UserAccommodations, "UserAccommodations", linkedEntity);
           
            IReadOnlyDictionary<string, object> result = linkedEntity;
            return result;
        }

        protected override async Task AttachLinkedEntityAsync(Accommodation entity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            entity.AccommodationFacilities = new List<AccommodationFacilityLink>();
            await FillEntityCollection(delegate (AccommodationFacilityLink item) { entity.AccommodationFacilities.Add(item); }, "AccommodationFacilities", linkedEntity, cancellationToken);

            await FillEntityField(entity.AccommodationType, entity.AccommodationTypeId, "AccommodationType", linkedEntity, cancellationToken);

            await FillEntityField(entity.Address, entity.AddressId ?? 0, "Address", linkedEntity, cancellationToken);

            entity.Rooms = new List<Room>();
            await FillEntityCollection(delegate (Room item) { entity.Rooms.Add(item); }, "Rooms", linkedEntity, cancellationToken);

            entity.UserAccommodations = new List<UserAccommodation>();
            await FillEntityCollection(delegate (UserAccommodation item) { entity.UserAccommodations.Add(item); }, "UserAccommodations", linkedEntity, cancellationToken);
        }
        #endregion
    }
}