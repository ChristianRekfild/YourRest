using YourRest.Application.Exceptions;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityByIdUseCase : IGetCityByIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }


        public async Task<CityDTO> Execute(int id)
        {
            var city = await _cityRepository.GetAsync(id);

            if (city is null) throw new EntityNotFoundException($"City with id {id} not found");

            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name
            };

        }
    }
}
