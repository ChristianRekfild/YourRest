using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityByRegionIdUseCase : IGetCityByRegionIdUseCase
    {
        private readonly ICityRepository _cityRepository;
        private readonly IRegionRepository _regionRepository;

        public GetCityByRegionIdUseCase(ICityRepository cityRepository, IRegionRepository regionRepository)
        {
            _cityRepository = cityRepository;
            _regionRepository = regionRepository;
        }


        public async Task<IEnumerable<CityDTO>> Execute(int regionId)
        {
            var region = _regionRepository.GetAsync(regionId).Result;
            if (region is null) throw new RegionNotFoundException($"Region with id {regionId} not found");

            var cities = _cityRepository.FindAsync(x => x.RegionId == regionId).Result.ToList();
            if (cities.Count == 0) throw new CityNotFoundException($"Cities in region {regionId} not found");

            return cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
    }
}
