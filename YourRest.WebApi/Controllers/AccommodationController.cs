﻿using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.Accommodations;
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
        private readonly IDeleteAddressFromAccommodationUseCase _deleteAddressFromAccommodationUseCase;
        private readonly IEditAccommodationsUseCase _editAccommodationUseCase;
        private readonly IRemoveAccommodationsUseCase _removeAccommodationUseCase;
        private readonly IGetAccommodationsUseCase _getAccommodation;
        private readonly IGetAccommodationsByIdUseCase _getAccommodationByIdUseCase;

        public AccommodationController(
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase,
            IFetchAccommodationsUseCase fetchAccommodationsUseCase,
            ICreateAccommodationUseCase createAccommodationUseCase,
            IDeleteAddressFromAccommodationUseCase deleteAddressFromAccommodationUseCase,
            IEditAccommodationsUseCase editAccommodationUseCase,
            IRemoveAccommodationsUseCase removeAccommodationUseCase,
            IGetAccommodationsUseCase getAccommodation,
            IGetAccommodationsByIdUseCase getAccommodationByIdUseCase
            )
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
            _fetchAccommodationsUseCase = fetchAccommodationsUseCase;
            _createAccommodationUseCase = createAccommodationUseCase;
            _deleteAddressFromAccommodationUseCase = deleteAddressFromAccommodationUseCase;
            _editAccommodationUseCase = editAccommodationUseCase;
            _removeAccommodationUseCase = removeAccommodationUseCase;
            _getAccommodation = getAccommodation;
            _getAccommodationByIdUseCase = getAccommodationByIdUseCase;
        }

        [HttpPost("api/accommodations/{accommodationId}/address", Name = "AddAddressToAccommodationAsync")]
        public async Task<IActionResult> AddAddressToAccommodationAsync([FromRoute] int accommodationId, [FromBody] AddressDto addressWithIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressWithIdDto);

            return CreatedAtRoute(nameof(AddAddressToAccommodationAsync), result);
        }
        
        [HttpDelete("api/accommodations/{accommodationId}/address/{addressId}", Name = "DeleteAddressAccommodationLinkAsync")]
        public async Task<IActionResult> DeleteAddressFromAccommodationAsync([FromRoute] int accommodationId, [FromRoute] int addressId)
        {
            await _deleteAddressFromAccommodationUseCase.Execute(accommodationId, addressId);

            return Ok($"Address {addressId} from Accommodation {accommodationId} has been successfully deleted.");
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
        [Route("api/accommodations/{accommodationId}")]
        public async Task<IActionResult> Delete([FromRoute] int accommodationId)
        {

            await _removeAccommodationUseCase.ExecuteAsync(accommodationId, HttpContext.RequestAborted);
            return Ok("The Accommodation has been Deleted");
        }

        [HttpPut]
        [Route("api/accommodations", Name = "EditAccommodation")]
        public async Task<IActionResult> Put([FromBody] EditAccommodationDto accommodationEdit)
        {
            return Ok(await _editAccommodationUseCase.ExecuteAsync(accommodationEdit, HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("api/accommodations")]
        public async Task<IActionResult> GetAccommodation()
        {
            return Ok(await _getAccommodation.ExecuteAsync(HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("api/accommodations/{accommodationId}")]
        public async Task<IActionResult> GetAccommodationById([FromRoute] int accommodationId)
        {
            return Ok(await _getAccommodationByIdUseCase.ExecuteAsync(accommodationId, HttpContext.RequestAborted));
        }
    }
}