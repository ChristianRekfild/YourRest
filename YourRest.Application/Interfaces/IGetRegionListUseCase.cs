using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetRegionListUseCase
    {
        Task<IEnumerable<RegionDto>> Execute();
    }
}
