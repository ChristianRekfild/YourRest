using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityByRegionIdUseCase : IGetCityByRegionIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByRegionIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        
        public async Task<IEnumerable<CityDTO>> Execute(int regionId)
        {
            var cities = await _cityRepository.FindAsync(x => x.RegionId == regionId);

            if (!cities.Any())
            {
                return new List<CityDTO>();
            }

            return cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
    }
}
