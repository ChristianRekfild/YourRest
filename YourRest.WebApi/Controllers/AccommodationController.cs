using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using YourRest.WebApi.Responses;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operator/accommodation")]
    public class AccommodationController : ControllerBase
    {
        private readonly IAddAddressToAccommodationUseCase _addAddressToAccommodationUseCase;

        public AccommodationController(IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase)
        {
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
        }

        [HttpPost("{accommodationId}/address", Name = "AddAddressToAccommodationAsync")]
        public async Task<IActionResult> AddAddressToAccommodationAsync(int accommodationId, [FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

            return CreatedAtRoute(nameof(AddAddressToAccommodationAsync), result);
        }
    }
}