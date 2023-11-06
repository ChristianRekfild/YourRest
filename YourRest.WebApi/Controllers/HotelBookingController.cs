using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Domain.Entities;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/hotelbooking")]
    [FluentValidationAutoValidation]
    public class HotelBookingController : Controller
    {
        private readonly ICreateHotelBookingUseCase _createHotelBookingUseCase;

        public HotelBookingController(ICreateHotelBookingUseCase createHotelBookingUseCase)
        {
            _createHotelBookingUseCase = createHotelBookingUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> PostHotelBookingAsync([FromBody] HotelBookingDto createHotelBookingUseCase)
        {
            var createdHotelBooking = await _createHotelBookingUseCase.ExecuteAsync(createHotelBookingUseCase, HttpContext.RequestAborted);
            return CreatedAtAction(nameof(PostHotelBookingAsync), createdHotelBooking);
        }
    }
}
