using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetRegionListUseCase
    {
        Task<IEnumerable<RegionDto>> Execute();
    }
}
