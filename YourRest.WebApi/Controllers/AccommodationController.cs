using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.CustomErrors;
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

        [HttpPost("{accommodationId}/address")]
        public async Task<IActionResult> AddAddressToAccommodationAsync([FromBody] AddressDto addressDto, [FromRoute] int accommodationId)
        {            
            try
            {
                var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

                return CreatedAtAction(nameof(AddAddressToAccommodationAsync), new { id = result.Id }, result);
            }
            catch (Exception exception) when (exception is AccommodationNotFoundException || exception is CityNotFoundException)
            {
                return NotFound(new ErrorResponse{ Message = exception.Message});
            }
        }
    }
}