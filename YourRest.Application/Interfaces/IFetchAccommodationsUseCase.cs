using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces
{
    public interface IFetchAccommodationsUseCase
    {
        Task<IEnumerable<AccommodationExtendedDto>> Execute(FetchAccommodationsViewModel viewModel);
    }
}
