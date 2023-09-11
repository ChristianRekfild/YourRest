using SharedKernel.Domain.Entities;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Controllers
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
        [Route("api/cities/get/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _getCityByIdUseCase.execute(id);
            return Ok(city);
        }

    }
}