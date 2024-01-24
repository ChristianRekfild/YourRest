using YourRest.Application.Dto.ViewModels;
using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Application.Dto.Mappers
{
    public interface IAccommodationMapper
    {
        AccommodationFilterCriteriaDto MapToFilterCriteria(FetchAccommodationsViewModel viewModel);
    }
}