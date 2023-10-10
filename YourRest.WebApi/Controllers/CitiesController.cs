using Microsoft.AspNetCore.Mvc;
using System;
using YourRest.Application.CustomErrors;
using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    //[Route("api/cities")]
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
            try
            {
                var city = await _getCityByIdUseCase.Execute(id);
                return Ok(city);
            }
            catch (CityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpGet]
        [Route("api/cities")]
        public async Task<IActionResult> GetCities([FromQuery] int? regionId = null, [FromQuery] int? countryId = null)
        {
            try
            {
                if (regionId.HasValue)
                {
                    var citiesByRegion = await _getCityByRegionIdUseCase.Execute(regionId.Value);
                    return Ok(citiesByRegion);
                }
                else if (countryId.HasValue)
                {
                    var citiesByCountry = await _getCityByCountryIdUseCase.Execute(countryId.Value);
                    return Ok(citiesByCountry);
                }
                else
                {
                    var allCities = await _getCityListUseCase.Execute();
                    return Ok(allCities);
                }
            }
            catch (Exception exception) when (exception is CountryNotFoundException || exception is CityNotFoundException) { 
                return NotFound(exception.Message);
            }
        }
    }
}