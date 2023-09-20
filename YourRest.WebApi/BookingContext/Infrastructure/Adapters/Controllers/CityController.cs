﻿using SharedKernel.Domain.Entities;
using YourRest.WebApi.BookingContext.Application.UseCases;
using YourRest.WebApi.BookingContext.Application.Ports;
using Microsoft.AspNetCore.Mvc;
using YourRest.WebApi.BookingContext.Application.CustomErrors;

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
        [Route("api/cities/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            try
            {
                var city = await _getCityByIdUseCase.Execute(id);
                return Ok(city);
            } catch (CityNotFountException) // город с таким Id не был найден
            {
                return NotFound();
            } catch (Exception ex) // на случай неведомого и непонятного
            { 
                return BadRequest(ex.Message);
            }
            
        }

    }
}