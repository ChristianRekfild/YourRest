using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using YourRest.WebApi.Responses;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operator/accommodation")]
    [FluentValidationAutoValidation]
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
        public async Task<IActionResult> AddAddressToAccommodationAsync([FromRoute] int accommodationId, [FromBody] AddressDto addressDto)
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