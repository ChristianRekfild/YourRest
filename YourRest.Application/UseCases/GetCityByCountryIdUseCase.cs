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


        public async Task<IEnumerable<CityDTO>> Execute(int countryId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.GetAsync(countryId, cancellationToken);
            if (country == null)
            {
                throw new EntityNotFoundException($"Country with id {countryId} not found");
            }

            var regions = (await _regionRepository.FindAsync(x => x.CountryId == countryId, cancellationToken)).Select(c => c.Id).ToList();
            if (!regions.Any())
            {
                throw new EntityNotFoundException($"Regions in country {countryId} not found");
            }

            IEnumerable<City> cities;
            if (isOnlyFavorite)
            {
                cities = await _cityRepository.FindAsync(x => regions.Contains(x.RegionId) && x.IsFavorite, cancellationToken);
            }
            else
            {
                cities = await _cityRepository.FindAsync(x => regions.Contains(x.RegionId), cancellationToken);
            }

            if (!cities.Any())
            {
                throw new EntityNotFoundException($"Cities in country {countryId} not found");
            }

            var resultCitiesList = cities.Select(c => new CityDTOWithLastPhoto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsFavorite = c.IsFavorite,
                LastPhoto = c.CityPhotos.Select(photo => new PhotoPathResponseDto
                {
                    FilePath = photo.FilePath
                }).LastOrDefault()
            }).ToList();


            return resultCitiesList;
        }
    }
}
