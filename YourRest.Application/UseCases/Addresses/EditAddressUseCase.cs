using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class EditAddressUseCase : IEditAddressUseCase
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public EditAddressUseCase(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<AddressWithIdDto> ExecuteAsync(AddressWithIdDto addressWithIdDto, CancellationToken cancellationToken)
        {
            var address = (await addressRepository.FindAsync(a => a.Id == addressWithIdDto.Id, cancellationToken)).FirstOrDefault();

            if(address == null)
            {
                throw new Exception($"Адрес с Id {addressWithIdDto.Id} не найден.");
            }

            var addressProperties = typeof(Infrastructure.Core.Contracts.Models.AddressDto).GetProperties();
            foreach(var property in addressProperties)
            {
                property.SetValue(address, typeof(AddressWithIdDto).GetProperty(property.Name));
            }

            address = await addressRepository.UpdateAsync(address, true, cancellationToken);

            return mapper.Map<AddressWithIdDto>(address);
        }
    }
}
