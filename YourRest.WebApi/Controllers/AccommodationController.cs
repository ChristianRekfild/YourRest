using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.Security.Claims;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Accommodation;
using YourRest.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.UseCases.Room;
using YourRest.Domain.Entities;
using YourRest.Application.Interfaces.Room;
using System.Web.Http;
using YourRest.Application.UseCases.Accommodation;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class AccommodationController : ControllerBase
    {
        private readonly IAddAddressToAccommodationUseCase _addAddressToAccommodationUseCase;
        private readonly IFetchAccommodationsUseCase _fetchAccommodationsUseCase;
        private readonly ICreateAccommodationUseCase _createAccommodationUseCase;
        private readonly IEditAccommodationsUseCase _editAccommodationUseCase;
        private readonly IRemoveAccommodationsUseCase _removeAccommodationUseCase;


        public AccommodationController(
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase,
            IFetchAccommodationsUseCase fetchAccommodationsUseCase,
            ICreateAccommodationUseCase createAccommodationUseCase,
            IEditAccommodationsUseCase editAccommodationUseCase,
            IRemoveAccommodationsUseCase removeAccommodationUseCase
            )
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
            _fetchAccommodationsUseCase = fetchAccommodationsUseCase;
            _createAccommodationUseCase = createAccommodationUseCase;
            _editAccommodationUseCase = editAccommodationUseCase;
            _removeAccommodationUseCase = removeAccommodationUseCase;
        }

        [HttpPost("api/operators/accommodations/{accommodationId}/address", Name = "AddAddressToAccommodationAsync")]
        public async Task<IActionResult> AddAddressToAccommodationAsync([FromRoute] int accommodationId, [FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

            return CreatedAtRoute(nameof(AddAddressToAccommodationAsync), result);
        }

        [HttpPost("api/accommodations", Name = "FetchAccommodationsByFilter")]
        public async Task<IActionResult> FetchHotels([FromBody] FetchAccommodationsViewModel fetchHotelsViewModel)
        {
            if (fetchHotelsViewModel.DateFrom == default || fetchHotelsViewModel.DateTo == default || fetchHotelsViewModel.Adults == default)
            {
                return BadRequest("date_from, date_to, and adults are required fields.");
            }

            var hotels = await _fetchAccommodationsUseCase.ExecuteAsync(fetchHotelsViewModel, HttpContext.RequestAborted);
            return Ok(hotels);
        }

        [HttpDelete]
        [Route("api/accommodation/{accommodationId}")]
        public async Task<IActionResult> Delete([FromRoute] int accommodationId)
        {

            await _removeAccommodationUseCase.ExecuteAsync(accommodationId, HttpContext.RequestAborted);
            return Ok("The Accommodation has been Deleted");
        }

        [Authorize]
        [HttpPost]
        [Route("api/accommodation")]
        public async Task<IActionResult> Post([FromBody] CreateAccommodationDto accommodationExtendedDto)
        {
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }

            var createdAccommodation = await _createAccommodationUseCase.ExecuteAsync(accommodationExtendedDto, sub, HttpContext.RequestAborted);
            return CreatedAtAction(nameof(Post), createdAccommodation);
        }



        //[HttpPut]
        //[Route("api/accommodation/{accommodationId}")]
        //public async Task<IActionResult> Put([FromBody] AccommodationDto accommodationDto, int accommodationId)
        //{

        //    roomWithId.Id = route.Id;
        //    await editRoomUseCase.ExecuteAsync(roomWithId, accommodationId, HttpContext.RequestAborted);
        //    return Ok("The room has been edited");
        //}



    }
}