using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces.AccommodationFacility;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class AccommodationFacilityController : ControllerBase
    {
        private readonly IGetAllAccommodationFacilitiesUseCase _getAllAccommodationFacilitiesUseCase;
        private readonly IGetAccommodationFacilityByAccommodationIdUseCase _getFacilitiesByAccommodationIdUseCase;
        private readonly IAddAccommodationFacilityUseCase _addAccommodationFacilityUseCase;
        private readonly IRemoveAccommodationFacilityUseCase _removeAccommodationFacilityUseCase;

        public AccommodationFacilityController(
            IGetAllAccommodationFacilitiesUseCase getAllAccommodationFacilitiesUseCase,
            IGetAccommodationFacilityByAccommodationIdUseCase getFacilitiesByAccommodationIdUseCase,
            IAddAccommodationFacilityUseCase addAccommodationFacilityUseCase,
            IRemoveAccommodationFacilityUseCase removeAccommodationFacilityUseCase
        ) {
            _getAllAccommodationFacilitiesUseCase = getAllAccommodationFacilitiesUseCase;
            _getFacilitiesByAccommodationIdUseCase = getFacilitiesByAccommodationIdUseCase;
            _addAccommodationFacilityUseCase = addAccommodationFacilityUseCase;
            _removeAccommodationFacilityUseCase = removeAccommodationFacilityUseCase;
        }
        [HttpGet]
        [Route("api/accommodation-facilities")]
        public async Task<IActionResult> GetAllAccommodationFacilitiesAsync()
        {
            return Ok(await _getAllAccommodationFacilitiesUseCase.ExecuteAsync(HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("api/accommodation/{id}/facilities")]
        public async Task<IActionResult> GetFacilitiesByAccommodationIdAsync([FromRoute] RouteViewModel route)
        {
            return Ok(await _getFacilitiesByAccommodationIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [HttpPost]
        [Route("api/accommodation/{id}/facility")]
        public async Task<IActionResult> AddFacilityToAccommodationAsync([FromRoute] RouteViewModel route, [FromBody] AccommodationFacilityIdDto accommodationFacility)
        {
            await _addAccommodationFacilityUseCase.ExecuteAsync(route.Id, accommodationFacility, HttpContext.RequestAborted);
            return Ok("The accommodation facility has been added to current accommodation");
        }
        
        [HttpDelete]
        [Route("api/accommodation/{id}/facility/{facility-id}")]
        public async Task<IActionResult> RemoveAccommodationFacilityLinkAsync([FromRoute] DeleteAccommodationFacilityViewModel route)
        {
            await _removeAccommodationFacilityUseCase.ExecuteAsync(route.Id, route.FacilityId, HttpContext.RequestAborted);
            return Ok($"AccommodationFacility id:{route.Id} has been removed from the current accommodation");
        }
    }
}
