using YourRest.Application.Dto.ViewModels;
using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Application.Dto.Mappers{
    public class AccommodationMapper : IAccommodationMapper
    {
        public AccommodationFilterCriteriaDto MapToFilterCriteria(FetchAccommodationsViewModel viewModel)
        {
            return new AccommodationFilterCriteriaDto
            {
                CountryIds = viewModel.CountryIds,
                CityIds = viewModel.CityIds,
                AccommodationTypesIds = viewModel.AccommodationTypesIds,
            };
        }

    }
}