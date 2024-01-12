using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class GetAddressUseCase : IGetAddressUseCase
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;
        public GetAddressUseCase(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> ExecuteAsync(CancellationToken cancellationToken)
            => mapper.Map<IEnumerable<AddressDto>>(await addressRepository.GetAllAsync(cancellationToken));
    }
}
