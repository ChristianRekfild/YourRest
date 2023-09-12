using SharedKernel.Domain.Entities;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Controllers
{
    [ApiController]
    [Route("api/regions")]
    public class RegionController : ControllerBase
    {
        private readonly IGetRegionListUseCase _getRegionListUseCase;

        public RegionController(IGetRegionListUseCase getRegionListUseCase)
        {
            _getRegionListUseCase = getRegionListUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _getRegionListUseCase.execute();
            return Ok(regions);
        }
    }
}