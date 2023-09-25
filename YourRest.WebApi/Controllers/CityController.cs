using YourRest.Application.CustomErrors;
using YourRest.Application.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    //[Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly IGetCityByIdUseCase _getCityByIdUseCase;

        public CityController(IGetCityByIdUseCase getCityByIdUseCase)
        {
            _getCityByIdUseCase = getCityByIdUseCase;
        }

        [HttpGet]
        [Route("api/cities/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                var city = await _getCityByIdUseCase.Execute(id);
                return Ok(city);
            } catch (CityNotFoundException exception) // город с таким Id не был найден
            {
                return NotFound(exception.Message);
            }
            
        }

    }
}