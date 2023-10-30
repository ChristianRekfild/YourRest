using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByIdUseCase
    {
        Task<CityDTO> Execute(int id);
    }
}
