using YourRest.WebApi.BookingContext.Application.Dto;
using YourRest.WebApi.BookingContext.Application.Ports;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Application.UseCases
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
            var cities = await _cityRepository.GetCityListAsync();

            return cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }

    }
}
