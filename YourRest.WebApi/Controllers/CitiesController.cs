using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    //[Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly IGetCityListUseCase _getCityListUseCase;

        public CitiesController(IGetCityListUseCase getCityListUseCase)
        {
            _getCityListUseCase = getCityListUseCase;
        }
        
        [HttpGet]
        [Route("api/cities")]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _getCityListUseCase.Execute();
            return Ok(cities);
        }

    }
}