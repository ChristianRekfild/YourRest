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

        /// <summary>
        /// Возвращает список Городов по Региону, в котором они находятся
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cities/regionid={id}")]
        public async Task<IActionResult> GetCityByRegionId(int id)
        {
            try
            {
                var city = await _getCityByRegionIdUseCase.Execute(id);
                return Ok(city);
            }
            catch (RegionNotFoundException ex) { // города в данном регионе не были найдены
                return NotFound(ex.Message);
            }
            catch (CityNotFoundException ex) { // города в данном регионе не были найдены
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Возвращает список Городов по Сегиону, в котором они находятся
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cities/countryid={id}")]
        public async Task<IActionResult> GetCityByCountryId(int id)
        {
            try
            {
                var city = await _getCityByCountryIdUseCase.Execute(id);
                return Ok(city);
            }
            catch (CountryNotFoundException ex)
            { // города в данном регионе не были найдены
                return NotFound(ex.Message);
            }
            catch (RegionNotFoundException ex)
            { // города в данном регионе не были найдены
                return NotFound(ex.Message);
            }
            catch (CityNotFoundException ex)
            { // города в данном регионе не были найдены
                return NotFound(ex.Message);
            }
        }
    }
}