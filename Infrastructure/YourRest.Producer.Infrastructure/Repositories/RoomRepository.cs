using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomRepository : PgRepository<Room, int, RoomDto>, IRoomRepository
    {
        public RoomRepository(SharedDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        #region AddAsync
        protected override IReadOnlyDictionary<string, object> DetachLinkedEntityAsync(Room entity)
        {
            Dictionary<string, object> linkedEntity = new();

            entity.Accommodation = SaveLinkedEntityProperty(entity.Accommodation, "Accommodation", linkedEntity);

            SaveLinkedEntityCollection(entity.RoomFacilities, "RoomFacilities", linkedEntity);

            entity.RoomType = SaveLinkedEntityProperty(entity.RoomType, "RoomType", linkedEntity);
            
            IReadOnlyDictionary<string, object> result = linkedEntity;
            return result;
        }

        protected override async Task AttachLinkedEntityAsync(Room entity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            await FillEntityField(entity.Accommodation, entity.AccommodationId, "Accommodation", linkedEntity, cancellationToken);

            entity.RoomFacilities = new List<RoomFacility>();
            await FillEntityCollection(delegate (RoomFacility item) { entity.RoomFacilities.Add(item); }, "RoomFacilities", linkedEntity, cancellationToken);

            await FillEntityField(entity.RoomType, entity.RoomTypeId, "RoomType", linkedEntity, cancellationToken);            
        }
        #endregion

        public async Task<List<RoomDto>> GetRoomsByCityAndDatesAsync(DateOnly startDate, DateOnly endDate, int cityId, CancellationToken cancellation = default)
        {
            var roomsByCityId = this._dataContext.Set<Room>()
               .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Address.CityId == cityId);

            var roomsByCityIdInBooking = this._dataContext.Set<Room>()
                .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Address.CityId == cityId &&
                room.bookings.Any(b => (b.StartDate <= startDate && b.EndDate > startDate) ||
                    (b.StartDate < endDate && b.EndDate > endDate) ||
                    (startDate <= b.StartDate && b.EndDate <= endDate))
                );

            return _mapper.Map<List<RoomDto>>(await roomsByCityId
                .Except(roomsByCityIdInBooking)
                .AsQueryable()
                .ToListAsync(cancellation));
        }
        public async Task<List<RoomDto>> GetRoomsByAccommodationAndDatesAsync(DateOnly startDate, DateOnly endDate, int accommodationId, CancellationToken cancellation = default)
        {
            var roomsByAccommodationId = this._dataContext.Set<Room>()
               .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Id == accommodationId);

            var roomsByAccommodationIdInBooking = this._dataContext.Set<Room>()
                .Where(room => room.Accommodation.Address != null &&
                room.Accommodation.Id == accommodationId &&
                room.bookings.Any(b => (b.StartDate <= startDate && startDate < b.EndDate) ||
                (b.StartDate < endDate && endDate < b.EndDate) ||
                (startDate <= b.StartDate && b.EndDate <= endDate)));

            return _mapper.Map<List<RoomDto>>(await roomsByAccommodationId
                .Except(roomsByAccommodationIdInBooking)
                .AsQueryable()
                .ToListAsync(cancellation));
        }
    }
}
