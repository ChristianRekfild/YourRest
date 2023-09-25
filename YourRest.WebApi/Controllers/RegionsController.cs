using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
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
            var regions = await _getRegionListUseCase.Execute();
            return Ok(regions);
        }
    }
}