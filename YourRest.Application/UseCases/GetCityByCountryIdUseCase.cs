using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;

namespace YourRest.Application.UseCases
{
    public class GetCityByCountryIdUseCase : IGetCityByCountryIdUseCase
    {
        private readonly ICityRepository _cityRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICountryRepository _countryRepository;

        public GetCityByCountryIdUseCase(IRegionRepository regionRepository, 
            ICityRepository cityRepository, ICountryRepository countryRepository)
        {
            _cityRepository = cityRepository;
            _regionRepository = regionRepository;
            _countryRepository = countryRepository;
        }


        public async Task<IEnumerable<CityDTOWithLastPhoto>> Execute(int countryId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetAsync(countryId, cancellationToken);
            if (country == null)
            {
                throw new EntityNotFoundException($"Country with id {countryId} not found");
            }

            var cities = await _cityRepository.GetCitiesWithPhotosByCountryAsync(countryId, isOnlyFavorite, cancellationToken);

            return cities.Select(c => new CityDTOWithLastPhoto
            {
                City = new CityDTO{
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsFavorite = c.IsFavorite
                },
                LastPhoto = c.CityPhotos.OrderByDescending(photo => photo.Id)
                    .Select(photo => new PhotoPathResponseDto
                    {
                        FilePath = photo.FilePath
                    })
                    .FirstOrDefault()
            }).ToList();
        }

    }
}
