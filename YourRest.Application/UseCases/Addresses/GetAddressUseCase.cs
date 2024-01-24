using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Infrastructure.Core.Contracts.Repositories;

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

        public async Task<IEnumerable<AddressWithIdDto>> ExecuteAsync(CancellationToken cancellationToken)
            => mapper.Map<IEnumerable<AddressWithIdDto>>(await addressRepository.GetAllAsync(cancellationToken));
    }
}
