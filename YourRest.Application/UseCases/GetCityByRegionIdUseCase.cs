using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.UseCases
{
    public class GetCityByRegionIdUseCase : IGetCityByRegionIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByRegionIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        
        public async Task<IEnumerable<CityDTO>> Execute(int regionId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            IEnumerable<City> cities;

            if (isOnlyFavorite)
            {
                cities = await _cityRepository.FindAsync(x => x.RegionId == regionId && x.IsFavorite, cancellationToken);
            }
            else
            {
                cities = await _cityRepository.FindAsync(x => x.RegionId == regionId, cancellationToken);
            }

            return cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsFavorite = c.IsFavorite
            }).ToList();
        }

    }
}
