using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.Security.Claims;
using YourRest.Application.Dto.Models;
using Microsoft.AspNetCore.Routing;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.UseCases.Room;
using YourRest.Domain.Entities;
using YourRest.Application.Interfaces.Room;
using YourRest.Application.Interfaces.Accommodations;

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
        private readonly IGetAccommodationsUseCase _getAccommodation;
        private readonly IGetAccommodationsByIdUseCase _getAccommodationByIdUseCase;



        public AccommodationController(
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase,
            IFetchAccommodationsUseCase fetchAccommodationsUseCase,
            ICreateAccommodationUseCase createAccommodationUseCase,
            IEditAccommodationsUseCase editAccommodationUseCase,
            IRemoveAccommodationsUseCase removeAccommodationUseCase,
            IGetAccommodationsUseCase getAccommodation,
            IGetAccommodationsByIdUseCase getAccommodationByIdUseCase
            )
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
            _fetchAccommodationsUseCase = fetchAccommodationsUseCase;
            _createAccommodationUseCase = createAccommodationUseCase;
            _editAccommodationUseCase = editAccommodationUseCase;
            _removeAccommodationUseCase = removeAccommodationUseCase;
            _getAccommodation = getAccommodation;
            _getAccommodationByIdUseCase = getAccommodationByIdUseCase;
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

        [HttpDelete]
        [Route("api/accommodation/{accommodationId}")]
        public async Task<IActionResult> Delete([FromRoute] int accommodationId)
        {

            await _removeAccommodationUseCase.ExecuteAsync(accommodationId, HttpContext.RequestAborted);
            return Ok("The Accommodation has been Deleted");
        }

        [HttpPut]
        [Route("api/accommodation/{accommodationId}")]
        public async Task<IActionResult> Put([FromBody] AccommodationExtendedDto accommodationExtendedDto, int accommodationId)
        {
            return Ok(await _editAccommodationUseCase.ExecuteAsync(accommodationExtendedDto, HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("api/accommodation")]
        public async Task<IActionResult> GetAccommodation()
        {
            return Ok(await _getAccommodation.ExecuteAsync(HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("api/accommodation/{accommodationId}")]
        public async Task<IActionResult> GetAccommodationById([FromRoute] int accommodationId)
        {
            return Ok(await _getAccommodationByIdUseCase.ExecuteAsync(accommodationId, HttpContext.RequestAborted));
        }
    }
}