using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Interfaces;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    //[Route("api/cities")]
    [FluentValidationAutoValidation]
    public class CitiesController : ControllerBase
    {
        private readonly IGetCityListUseCase _getCityListUseCase;
        private readonly IGetCityByIdUseCase _getCityByIdUseCase;
        private readonly IGetCityByRegionIdUseCase _getCityByRegionIdUseCase;
        private readonly IGetCityByCountryIdUseCase _getCityByCountryIdUseCase;

        public CitiesController(
            IGetCityListUseCase getCityListUseCase,
            IGetCityByIdUseCase getCityByIdUseCase,
            IGetCityByRegionIdUseCase getCityByRegionIdUseCase,
            IGetCityByCountryIdUseCase getCityByCountryIdUseCase
            )
        {
            _getCityListUseCase = getCityListUseCase;
            _getCityByIdUseCase = getCityByIdUseCase;
            _getCityByRegionIdUseCase = getCityByRegionIdUseCase;
            _getCityByCountryIdUseCase = getCityByCountryIdUseCase;
        }

        [HttpGet]
        [Route("api/cities/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _getCityByIdUseCase.Execute(id);
            return Ok(city);
        }

        [HttpGet]
        [Route("api/cities")]
        public async Task<IActionResult> GetCities([FromQuery] int? regionId = null, [FromQuery] int? countryId = null, [FromQuery] bool isOnlyFavorite = false)
        {
            if (regionId.HasValue)
            {
                var citiesByRegion = await _getCityByRegionIdUseCase.Execute(regionId.Value, isOnlyFavorite, HttpContext.RequestAborted);
                return Ok(citiesByRegion);
            }

            if (countryId.HasValue)
            {
                var citiesByCountry = await _getCityByCountryIdUseCase.Execute(countryId.Value, isOnlyFavorite, HttpContext.RequestAborted);
                return Ok(citiesByCountry);
            }
            var allCities = await _getCityListUseCase.Execute(isOnlyFavorite, HttpContext.RequestAborted);
            return Ok(allCities);
        }
    }
}