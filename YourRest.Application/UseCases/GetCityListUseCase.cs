using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityListUseCase : IGetCityListUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityListUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityDTO>> Execute(bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            IEnumerable<City> cities;

            if (isOnlyFavorite)
            {
                cities = await _cityRepository.FindAsync(a => a.IsFavorite, cancellationToken);
            }
            else
            {
                cities = await _cityRepository.GetAllAsync(cancellationToken);
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
