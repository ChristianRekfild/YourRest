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
            var accommodationTask = _accommodationRepository.GetAsync(accommodationId);
            if (accommodationTask == null)
            {
                throw new AccommodationNotFoundException(accommodationId);
            }
            var accommodation = await accommodationTask;

            var cityId = addressDto.CityId;
            var cityTask = _cityRepository.GetAsync(cityId);
            if (cityTask == null)
            {
                throw new CityNotFoundException($"Country with id {cityId} not found");
            }

            var city = await cityTask;

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