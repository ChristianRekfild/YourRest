using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Models;

namespace YourRest.Application.Dto.Mappers{
    public class AccommodationMapper : IAccommodationMapper
    {
        public AccommodationFilterCriteria MapToFilterCriteria(FetchAccommodationsViewModel viewModel)
        {
            return new AccommodationFilterCriteria
            {
                CountryIds = viewModel.CountryIds,
                CityIds = viewModel.CityIds,
                AccommodationTypesIds = viewModel.AccommodationTypesIds,
            };
        }

    }
}