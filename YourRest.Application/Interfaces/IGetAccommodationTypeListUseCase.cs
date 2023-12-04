using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetAccommodationTypeListUseCase
    {
        Task<IEnumerable<AccommodationTypeDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
