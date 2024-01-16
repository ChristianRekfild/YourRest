using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.Security.Claims;
using YourRest.Application.Dto.Models;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class AccommodationController : ControllerBase
    {
        private readonly IAddAddressToAccommodationUseCase _addAddressToAccommodationUseCase;
        private readonly IFetchAccommodationsUseCase _fetchAccommodationsUseCase;
        private readonly ICreateAccommodationUseCase _createAccommodationUseCase;

        public AccommodationController(
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase,
            IFetchAccommodationsUseCase fetchAccommodationsUseCase,
            ICreateAccommodationUseCase createAccommodationUseCase
            )
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
            _fetchAccommodationsUseCase = fetchAccommodationsUseCase;
            _createAccommodationUseCase = createAccommodationUseCase;
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
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }

            var hotels = await _fetchAccommodationsUseCase.ExecuteAsync(sub, fetchHotelsViewModel, HttpContext.RequestAborted);
            return Ok(hotels);
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
    }
}