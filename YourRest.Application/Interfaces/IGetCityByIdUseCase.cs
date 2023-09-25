using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByIdUseCase
    {
        Task<CityDTO> Execute(int id);
    }
}
