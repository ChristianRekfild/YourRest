using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/countries")]
    [FluentValidationAutoValidation]
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
            var countries = await _getCountryListUseCase.Execute();
            return Ok(countries);
        }
    }
}