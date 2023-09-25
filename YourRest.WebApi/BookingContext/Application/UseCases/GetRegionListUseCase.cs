using YourRest.WebApi.BookingContext.Application.Dto;
using YourRest.WebApi.BookingContext.Application.Ports;
using YourRest.WebApi.BookingContext.Domain.Ports;

namespace YourRest.WebApi.BookingContext.Application.UseCases
{
    public class GetRegionListUseCase : IGetRegionListUseCase
    {
        private readonly IRegionRepository _regionRepository;
        
        public GetRegionListUseCase(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<IEnumerable<RegionDto>> Execute()
        {
            var regions = await _regionRepository.GetAllAsync();

            return regions.Select(r => new RegionDto
            {
                Id = r.Id,
                Name = r.Name,
                CountryId = r.CountryId
            }).ToList();
        
        }
    }
}
