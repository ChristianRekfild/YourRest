using YourRest.WebApi.BookingContext.Application.Dto;

namespace YourRest.WebApi.BookingContext.Application.Ports
{
    public interface IGetRegionListUseCase
    {
        Task<IEnumerable<RegionDto>> Execute();
    }
}
