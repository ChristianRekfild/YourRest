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
        private readonly IGetAccommodationByIdUseCase _getAccommodationByIdUseCase;

        public AccommodationController(IGetAccommodationByIdUseCase getAccommodationByIdUseCase,
            IAddAddressToAccommodationUseCase addAddressToAccommodationUseCase)
        {
            _getAccommodationByIdUseCase = getAccommodationByIdUseCase;
            _addAddressToAccommodationUseCase = addAddressToAccommodationUseCase;
        }

        [HttpGet("{accommodationId}")]
        public async Task<IActionResult> GetAccommodation(int accommodationId)
        {
            var accommodationDto = await _getAccommodationByIdUseCase.Execute(accommodationId);

            try
            {
                return Ok(accommodationDto);
            }
            catch (AccommodationNotFoundException ex)
            {
                return NotFound();
            }
            
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