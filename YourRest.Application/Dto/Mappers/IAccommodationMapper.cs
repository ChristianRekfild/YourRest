using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Models;

namespace YourRest.Application.Dto.Mappers
{
    public interface IAccommodationMapper
    {
        AccommodationFilterCriteria MapToFilterCriteria(FetchAccommodationsViewModel viewModel);
    }
}