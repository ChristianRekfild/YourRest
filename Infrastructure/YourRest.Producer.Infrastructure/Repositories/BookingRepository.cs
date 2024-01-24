using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class BookingRepository : PgRepository<Booking, int, BookingDto>, IBookingRepository
    {
        public BookingRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IReadOnlyDictionary<string, object> DetachLinkedEntityAsync(Booking entity)
        {
            Dictionary<string, object> linkedEntity = new();
            if (entity.Rooms.Any())
            {
                linkedEntity["Rooms"] = entity.Rooms;
                entity.Rooms = null;
            }
            if (entity.Customer != null)
            {
                linkedEntity["Customer"] = entity.Customer;
                entity.Customer = null;
            }
            IReadOnlyDictionary<string, object> result = linkedEntity;
            return result;
        }

        protected override async Task AttachLinkedEntityAsync(Booking entity, IReadOnlyDictionary<string, object> linkedEntity, CancellationToken cancellationToken)
        {
            if (linkedEntity.ContainsKey("Rooms") && linkedEntity["Rooms"] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);

                if (linkedEntity["Rooms"] is ICollection<Room> rooms)
                {
                    entity.Rooms = new List<Room>();
                    var existedRoomIds = rooms.Where(room => room.Id > 0).Select(room => room.Id);
                    var existedRooms = await this._dataContext.Set<Room>()
                        //.AsNoTracking()
                        .Where(r => existedRoomIds.Contains(r.Id))
                        .ToListAsync();
                    foreach(var room in existedRooms)
                    {
                        entity.Rooms.Add(room);
                    }
                    foreach(var room in rooms.Where(room => room.Id <= 0))
                    {
                        entity.Rooms.Add(room);
                    }
                }
            }
            if (linkedEntity.ContainsKey("Customer") && linkedEntity["Customer"] != null)
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
                if (linkedEntity["Customer"] is Customer customer)
                {
                    entity.Customer = customer;
                }
            }
        }
    }
}