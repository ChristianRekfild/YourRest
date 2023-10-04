using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class AddAddressToAccommodationUseCase : IAddAddressToAccommodationUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICityRepository _cityRepository;

        public AddAddressToAccommodationUseCase(
            IAccommodationRepository accommodationRepository,
            IAddressRepository addressRepository,
            ICityRepository cityRepository
        ) {
            _accommodationRepository = accommodationRepository;
            _addressRepository = addressRepository;
            _cityRepository = cityRepository;
        }

        public async Task<ResultDto> Execute(int accommodationId, AddressDto addressDto)
        {
            var accommodations = await _accommodationRepository.GetWithIncludeAsync(a => a.Id == accommodationId, a => a.Address);
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }

            if (accommodation.Address != null)
            {
                throw new AddressAlreadyExistsException(accommodationId);
            }

            var city = await _cityRepository.GetAsync(addressDto.CityId);
            if (city == null)
            {
                throw new CityNotFoundException($"City with id {addressDto.CityId} not found");
            }

            var addresses = await _addressRepository.FindAsync(
                a => a.Street == addressDto.Street 
                && a.ZipCode == addressDto.ZipCode 
                && a.Longitude == addressDto.Longitude 
                && a.Latitude == addressDto.Latitude
            );
            var address = addresses.FirstOrDefault();
            
            if (address == null)
            {
                address = new Address
                {
                    Street = addressDto.Street,
                    CityId = city.Id,
                    ZipCode = addressDto.ZipCode,
                    Longitude = addressDto.Longitude,
                    Latitude = addressDto.Latitude,
                };

                address = await _addressRepository.AddAsync(address);
                await _addressRepository.SaveChangesAsync();
            }

            accommodation.AddressId = address.Id;
            await _accommodationRepository.UpdateAsync(accommodation);
            await _accommodationRepository.SaveChangesAsync();

            return new ResultDto
            {
                Id = address.Id,
                Street = address.Street,
                ZipCode = address.ZipCode,
                Longitude = address.Longitude,
                Latitude = address.Latitude,
                CityId = address.CityId,
            };
        }

    }
}