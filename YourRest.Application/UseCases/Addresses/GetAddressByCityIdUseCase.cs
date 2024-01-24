using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class GetAddressByCityIdUseCase : IGetAddressByCityIdUseCase
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public GetAddressByCityIdUseCase(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AddressWithIdDto>> ExecuteAsync(int cityId, CancellationToken cancellationToken)
        {
            var addresses = await addressRepository.FindAsync(a => a.CityId == cityId, cancellationToken);
            return mapper.Map<IEnumerable<AddressWithIdDto>>(addresses);
        }
    }
}
