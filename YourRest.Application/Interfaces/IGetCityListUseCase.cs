using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityListUseCase
    {
        Task<IEnumerable<CityDTO>> Execute(bool isOnlyFavorite, CancellationToken cancellationToken);
    }
}
