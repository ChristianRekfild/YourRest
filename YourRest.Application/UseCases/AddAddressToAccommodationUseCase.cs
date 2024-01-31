using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;

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
            ICityRepository cityRepository)
        {
            _accommodationRepository = accommodationRepository;
            _addressRepository = addressRepository;
            _cityRepository = cityRepository;
        }

        public async Task<ResultDto> Execute(int accommodationId, AddressDto addressWithIdDto)
        {
            var accommodations = await _accommodationRepository.GetWithIncludeAsync(
                a => a.Id == accommodationId,
                cancellationToken: default,
                a => a.Address
            );
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {accommodationId} not found");
            }

            if (accommodation.Address != null)
            {
                throw new ValidationException($"Address for accommodation with id {accommodationId} already exists");
            }

            var city = await _cityRepository.GetAsync(addressWithIdDto.CityId);
            if (city == null)
            {
                throw new EntityNotFoundException($"City with id {addressWithIdDto.CityId} not found");
            }

            var addresses = await _addressRepository.FindAsync(
                a => a.Street == addressWithIdDto.Street
                && a.ZipCode == addressWithIdDto.ZipCode
                && a.Longitude == addressWithIdDto.Longitude
                && a.Latitude == addressWithIdDto.Latitude
            );
            var address = addresses.FirstOrDefault();

            if (address == null)
            {
                address = new Address
                {
                    Street = addressWithIdDto.Street,
                    CityId = city.Id,
                    ZipCode = addressWithIdDto.ZipCode,
                    Longitude = addressWithIdDto.Longitude,
                    Latitude = addressWithIdDto.Latitude,
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