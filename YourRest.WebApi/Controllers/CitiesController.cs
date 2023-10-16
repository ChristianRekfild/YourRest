using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;

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
        public async Task<IActionResult> GetCities([FromQuery] int? regionId = null, [FromQuery] int? countryId = null)
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
    }
}