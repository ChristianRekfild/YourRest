using YourRest.WebApi.BookingContext.Application.CustomErrors;
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


        public async Task<CityDTO> Execute(int id)
        {
            var city = await _cityRepository.GetCityByIdAsync(id);

            if (city is null) throw new CityNotFountException($"City with id {id} not found");

            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name
            };

        }
    }
}
