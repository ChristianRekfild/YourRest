using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using YourRest.WebApi.Responses;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class AccommodationController : ControllerBase
    {
        private readonly IAddAddressToAccommodationUseCase _addAddressToAccommodationUseCase;
        private readonly IFetchAccommodationsUseCase _fetchAccommodationsUseCase;

        public AccommodationController(
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase,
            IFetchAccommodationsUseCase fetchAccommodationsUseCase
            )
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
            _fetchAccommodationsUseCase = fetchAccommodationsUseCase;
        }

        [HttpPost("api/operator/accommodation/{accommodationId}/address", Name = "AddAddressToAccommodationAsync")]
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

            var hotels = await _fetchAccommodationsUseCase.Execute(fetchHotelsViewModel);
            return Ok(hotels);
        }
    }
}