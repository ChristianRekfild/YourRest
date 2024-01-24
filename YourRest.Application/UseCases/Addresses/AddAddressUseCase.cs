using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class AddAddressUseCase : IAddAddressUseCase
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public AddAddressUseCase(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<AddressWithIdDto> ExecuteAsync(AddressWithIdDto addressWithIdDto, CancellationToken cancellationToken)
        {

            if (addressWithIdDto == null)
            {
                throw new ArgumentNullException("AddressDto");
            }

            var address = await addressRepository.AddAsync(mapper.Map<Infrastructure.Core.Contracts.Models.AddressDto>(addressWithIdDto), true, cancellationToken);

            return mapper.Map<AddressWithIdDto>(address);
        }
    }
}
