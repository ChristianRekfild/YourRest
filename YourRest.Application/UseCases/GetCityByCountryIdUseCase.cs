using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using System.Linq;

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


        public async Task<IEnumerable<CityDTO>> Execute(int countryId)
        {
            var country = await _countryRepository.GetAsync(countryId);
            if (country == null) throw new CountryNotFoundException($"Country with id {countryId} not found");

            var regions = (await _regionRepository.FindAsync(x => x.CountryId == countryId)).Select(c => c.Id).ToList();
            if (regions.Count == 0) throw new RegionNotFoundException($"Regions in country {countryId} not found");

            var cities = (await _cityRepository.FindAsync(x => regions.Any(c => c == x.RegionId))).ToList(); ;
            if (cities.Count == 0) throw new CityNotFoundException($"Cities in country {countryId} not found");

            var resultCitiesList = cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            return resultCitiesList;
        }
    }
}
