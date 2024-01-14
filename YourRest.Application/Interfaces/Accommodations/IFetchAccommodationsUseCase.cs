using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodation
{
    public interface IFetchAccommodationsUseCase
    {
        Task<IEnumerable<AccommodationExtendedDto>> ExecuteAsync(string sub, FetchAccommodationsViewModel viewModel, CancellationToken cancellationToken);
    }
}
