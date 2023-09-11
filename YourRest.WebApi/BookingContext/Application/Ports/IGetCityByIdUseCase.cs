using YourRest.WebApi.BookingContext.Application.Dto;

namespace YourRest.WebApi.BookingContext.Application.Ports
{
    public interface IGetCityByIdUseCase
    {
        Task<CityDTO> execute(int id);
    }
}
