using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityByCountryIdUseCase
    {
        Task<IEnumerable<CityDTOWithLastPhoto>> Execute(int countryId, bool isOnlyFavorite, CancellationToken cancellationToken);
    }
}
