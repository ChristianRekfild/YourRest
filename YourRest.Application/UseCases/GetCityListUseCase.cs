using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityListUseCase : IGetCityListUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityListUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityDTO>> Execute()
        {
            var cities = await _cityRepository.GetAllAsync();

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
