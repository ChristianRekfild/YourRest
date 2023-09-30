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
            var accommodation = await _accommodationRepository.GetAsync(accommodationId);

            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
            
            if (accommodation.Address != null)
            {
                throw new AddressAlreadyExistsException(accommodationId);
            }

            var cityId = addressDto.CityId;
            var city = await _cityRepository.GetAsync(cityId);
            if (city == null)
            {
                throw new CityNotFoundException($"City with id {cityId} not found");
            }           

            var address = new Address
            {
                Street = addressDto.Street,
                CityId = city.Id,
                ZipCode = addressDto.ZipCode,
                Longitude = addressDto.Longitude,
                Latitude = addressDto.Latitude,
                AccommodationId = accommodation.Id
            };

            var savedAddress = await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();

            return new ResultDto
            {
                Id = savedAddress.Id,
                Street = savedAddress.Street,
                ZipCode = savedAddress.ZipCode,
                Longitude = savedAddress.Longitude,
                Latitude = savedAddress.Latitude,
                CityId = savedAddress.CityId,
                AccommodationId = savedAddress.AccommodationId
            };
        }
    }
}