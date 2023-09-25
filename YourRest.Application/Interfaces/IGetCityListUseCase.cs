using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityListUseCase
    {
        Task<IEnumerable<CityDTO>> Execute();
    }
}
