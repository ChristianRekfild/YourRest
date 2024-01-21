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
           
            if (entity.Accommodation != null)
            {
                linkedEntity["Accommodation"] = entity.Accommodation;
                entity.Accommodation = null;
            }

            if (entity.RoomFacilities.Any())
            {
                linkedEntity["RoomFacilities"] = entity.RoomFacilities;
                entity.RoomFacilities = null;
            }
            
            if (entity.RoomType != null)
            {
                linkedEntity["RoomType"] = entity.RoomType;
                entity.RoomType = null;
            }
            IReadOnlyDictionary<string, object> result = linkedEntity;
            return result;
        }

        protected override async Task AttachLinkedEntityAsync(Room entity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            if (linkedEntity.ContainsKey("Accommodation") && linkedEntity["Accommodation"] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
                if (linkedEntity["Accommodation"] is Accommodation accommodation)
                {
                    if (entity.AccommodationId <= 0 && accommodation.Id <= 0)
                    {
                        entity.Accommodation = accommodation;
                    }
                }
            }

            if (linkedEntity.ContainsKey("RoomFacilities") && linkedEntity["RoomFacilities"] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);

                if (linkedEntity["RoomFacilities"] is ICollection<RoomFacility> roomFacilities)
                {
                    entity.RoomFacilities = new List<RoomFacility>();
                    var existedRoomFacilityIds = roomFacilities
                        .Where(r => r.Id > 0)
                        .Select(r => r.Id);

                    var existedRoomFacilities = await this._dataContext.Set<RoomFacility>()
                        //.AsNoTracking()
                        .Where(r => existedRoomFacilityIds.Contains(r.Id))
                        .ToListAsync();

                    foreach (var room in existedRoomFacilities)
                    {
                        entity.RoomFacilities.Add(room);
                    }
                    foreach (var room in roomFacilities.Where(room => room.Id <= 0))
                    {
                        entity.RoomFacilities.Add(room);
                    }
                }
            }

            if (linkedEntity.ContainsKey("RoomType") && linkedEntity["RoomType"] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
                if (linkedEntity["RoomType"] is RoomType roomType)
                {
                    if(entity.RoomTypeId <= 0 && roomType.Id <= 0)
                    {
                        entity.RoomType = roomType;
                    }
                }
            }
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
