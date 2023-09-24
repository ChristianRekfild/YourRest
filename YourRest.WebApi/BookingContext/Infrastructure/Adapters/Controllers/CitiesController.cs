using SharedKernel.Domain.Entities;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Controllers
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