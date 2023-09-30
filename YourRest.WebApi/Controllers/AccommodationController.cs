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

        [HttpPost("{accommodationId}/address", Name = "AddAddressToAccommodationAsync")]
        public async Task<IActionResult> AddAddressToAccommodationAsync(int accommodationId, [FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _addAddressToAccommodationUseCase.Execute(accommodationId, addressDto);

                return CreatedAtRoute(nameof(AddAddressToAccommodationAsync), result);
            }
            catch (Exception exception) when (exception is AccommodationNotFoundException || exception is CityNotFoundException)
            {
                return NotFound(new ErrorResponse{ Message = exception.Message});
            } catch (AddressAlreadyExistsException exception)
            {
                return UnprocessableEntity(new ErrorResponse{ Message = exception.Message});
            }
        }
    }
}