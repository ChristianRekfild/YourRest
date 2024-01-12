using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

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

        public async Task<AddressDto> ExecuteAsync(AddressDto addressDto, CancellationToken cancellationToken)
        {
            var address = (await addressRepository.FindAsync(a => a.Id == addressDto.Id, cancellationToken)).FirstOrDefault();

            if(address == null)
            {
                throw new Exception($"Адрес с Id {addressDto.Id} не найден.");
            }

            var addressProperties = typeof(Address).GetProperties();
            foreach(var property in addressProperties)
            {
                property.SetValue(address, typeof(AddressDto).GetProperty(property.Name));
            }

            address = await addressRepository.UpdateAsync(address, true, cancellationToken);

            return mapper.Map<AddressDto>(address);
        }
    }
}
