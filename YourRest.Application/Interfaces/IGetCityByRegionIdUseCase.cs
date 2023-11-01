using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByRegionIdUseCase
    {
        Task<IEnumerable<CityDTO>> Execute(int regionId);
    }
}
