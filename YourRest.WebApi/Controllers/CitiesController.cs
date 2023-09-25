using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Interfaces;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    //[Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly IGetCityListUseCase _getCityListUseCase;
        private readonly IGetCityByIdUseCase _getCityByIdUseCase;

        public CitiesController(
            IGetCityListUseCase getCityListUseCase,
            IGetCityByIdUseCase getCityByIdUseCase)
        {
            _getCityListUseCase = getCityListUseCase;
            _getCityByIdUseCase = getCityByIdUseCase;
        }
        
        [HttpGet]
        [Route("api/cities")]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _getCityListUseCase.Execute();
            return Ok(cities);
        }

        [HttpGet]
        [Route("api/cities/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                var city = await _getCityByIdUseCase.Execute(id);
                return Ok(city);
            }
            catch (CityNotFoundException exception) // город с таким Id не был найден
            {
                return NotFound(exception.Message);
            }
        }

    }
}