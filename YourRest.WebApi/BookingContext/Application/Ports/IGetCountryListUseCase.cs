using YourRest.WebApi.BookingContext.Application.Dto;

namespace YourRest.WebApi.BookingContext.Application.Ports
{
    public interface IGetCountryListUseCase
    {
        Task<IEnumerable<CountryDto>> Execute();
    }
}
