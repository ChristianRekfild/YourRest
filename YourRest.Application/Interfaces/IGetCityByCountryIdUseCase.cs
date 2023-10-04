using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByCountryIdUseCase
    {
        Task<IEnumerable<CityDTO>> Execute(int countryId);
    }
}
