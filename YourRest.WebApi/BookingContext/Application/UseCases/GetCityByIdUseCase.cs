using YourRest.WebApi.BookingContext.Application.Dto;
using YourRest.WebApi.BookingContext.Application.Ports;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Application.UseCases
{
    public class GetCityByIdUseCase : IGetCityByIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }


        // TODO переделать
        public async Task<CityDTO> execute(int id)
        {
            var city = await _cityRepository.GetCityByIdAsync(id);

            if (city is null) return null;

            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name
            };

        }
    }
}
