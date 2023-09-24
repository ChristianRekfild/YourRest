using Microsoft.EntityFrameworkCore;
using YourRestDataAccesLayer.Abstractions;
using YourRestDomain.Entities;

namespace YourRestDataAccesLayer.Repositories
{
    public class AdditionalRoomServiceRepository : Repository<AdditionalRoomServiceEntity>, IAdditionalRoomServiceRepository
    {
        public AdditionalRoomServiceRepository(DbContext context) : base(context)
        {
        }
    }
}
