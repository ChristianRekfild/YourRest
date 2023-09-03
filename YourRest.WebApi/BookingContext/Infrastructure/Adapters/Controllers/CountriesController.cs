using SharedKernel.Domain.Entities;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.BookingContext.Infrastructure.Adapters.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IGetCountryListUseCase _getCountryListUseCase;

        public CountriesController(IGetCountryListUseCase getCountryListUseCase)
        {
            _getCountryListUseCase = getCountryListUseCase;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _getCountryListUseCase.execute();
            return Ok(countries);
        }
    }
}