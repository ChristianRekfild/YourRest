using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
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
            var regionsWithCountries = await _regionRepository.GetAllWithIncludeAsync(r => r.Country);

            return regionsWithCountries.Select(r => new RegionDto
            {
                Id = r.Id,
                Name = r.Name,
                CountryId = r.CountryId
            }).ToList();
        
        }
    }
}
