using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByRegionIdUseCase
    {
        Task<IEnumerable<CityDTOWithLastPhoto>> Execute(int regionId, bool isOnlyFavorite, CancellationToken cancellationToken);
    }
}
