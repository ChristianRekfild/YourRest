using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByRegionIdUseCase
    {
        Task<IEnumerable<CityDTO>> Execute(int regionId);
    }
}
