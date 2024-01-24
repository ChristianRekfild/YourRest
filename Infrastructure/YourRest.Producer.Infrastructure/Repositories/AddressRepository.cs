using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AddressRepository : PgRepository<Address, int, AddressDto>, IAddressRepository
{
    public AddressRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}