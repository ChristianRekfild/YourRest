using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCountryListUseCase
    {
        Task<IEnumerable<CountryDto>> Execute();
    }
}
