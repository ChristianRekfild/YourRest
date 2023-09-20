using YourRest.WebApi.BookingContext.Application.Dto;

namespace YourRest.WebApi.BookingContext.Application.Ports
{
    public interface IGetCityListUseCase
    {
        Task<IEnumerable<CityDTO>> Execute();
    }
}
