using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
   
    public class AccommodationTypeController : ControllerBase
    {

        private readonly IGetAccommodationTypeListUseCase _getAccommodationTypeListUseCase;



        public AccommodationTypeController(IGetAccommodationTypeListUseCase getAccommodationTypeListUseCase)
        {
            _getAccommodationTypeListUseCase = getAccommodationTypeListUseCase;
        }

        [HttpGet]
        [Route("api/accommodation-types")]
        public async Task<IActionResult> GetAllAccommodationTypes()
        {
            IEnumerable<AccommodationTypeDto> types = await _getAccommodationTypeListUseCase.Execute();
            return Ok(types);
        }
    }
}