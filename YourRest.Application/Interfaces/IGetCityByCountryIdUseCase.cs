using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByCountryIdUseCase
    {
        Task<IEnumerable<CityDTO>> Execute(int countryId);
    }
}
