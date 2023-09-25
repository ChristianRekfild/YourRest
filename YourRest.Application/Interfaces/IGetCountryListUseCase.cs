using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetCountryListUseCase
    {
        Task<IEnumerable<CountryDto>> Execute();
    }
}
