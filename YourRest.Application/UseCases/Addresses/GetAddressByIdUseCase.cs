using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class GetAddressByIdUseCase : IGetAddressByIdUseCase
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public GetAddressByIdUseCase(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<AddressDto> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var addresses = await addressRepository.FindAsync(a => a.Id == id, cancellationToken);
            if (addresses.Any())
            {
                return mapper.Map<AddressDto>(addresses.First());
            }
            return null;
        }
    }
}
