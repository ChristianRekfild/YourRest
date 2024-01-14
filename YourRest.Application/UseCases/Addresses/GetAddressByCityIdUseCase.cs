using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Domain.Repositories;

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

        public async Task<IEnumerable<AddressDto>> ExecuteAsync(int cityId, CancellationToken cancellationToken)
            => mapper.Map<IEnumerable<AddressDto>>(await addressRepository.FindAsync(a => a.CityId == cityId, cancellationToken));
    }
}
