using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AddressRepository : PgRepository<Address, int>, IAddressRepository
{
    public AddressRepository(SharedDbContext context) : base(context)
    {
    }
}