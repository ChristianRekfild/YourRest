using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Domain.ValueObjects.Addresses;

namespace YourRest.Application.UseCases
{
    public class AddAddressToAccommodationUseCase : IAddAddressToAccommodationUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ICityRepository _cityRepository;

        public AddAddressToAccommodationUseCase(IAccommodationRepository accommodationRepository, ICityRepository cityRepository)
        {
            _accommodationRepository = accommodationRepository;
            _cityRepository = cityRepository;
        }

        public async Task<ResultDto> Execute(int accommodationId, AddressDto addressDto)
        {
            var accommodation = await _accommodationRepository.GetAsync(accommodationId);

            if (accommodation == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
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
                City = city,
                ZipCode = addressDto.ZipCode,
                Longitude = addressDto.Longitude,
                Latitude = addressDto.Latitude,
                Type = AddressTypeVO.Create(addressDto.Type)
            };

            accommodation.Addresses.Add(address);
            await _accommodationRepository.UpdateAsync(accommodation);

            return new ResultDto
            {
                Id = address.Id
            };
        }
    }
}