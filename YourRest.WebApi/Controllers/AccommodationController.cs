using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.CustomErrors;

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

        [HttpPost("{accommodationId}/address")]
        public async Task<IActionResult> AddAddressToAccommodationAsync([FromBody] AddressDto addressDto, [FromQuery] int accommodationId)
        {
            var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);
            return CreatedAtAction(nameof(AddAddressToAccommodationAsync), new { id = result.Id }, result);
        }
    }
}