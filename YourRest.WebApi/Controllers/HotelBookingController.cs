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
    [FluentValidationAutoValidation]
    public class BookingController : Controller
    {
        private readonly ICreateBookingUseCase _createHotelBookingUseCase;

        public BookingController(ICreateBookingUseCase createHotelBookingUseCase)
        {
            _createHotelBookingUseCase = createHotelBookingUseCase;
        }

        [HttpPost]
        [Route("api/hotelbooking/")]
        public async Task<IActionResult> PostBookingAsync([FromBody] BookingDto createHotelBookingUseCase)
        {
            var createdHotelBooking = await _createHotelBookingUseCase.ExecuteAsync(createHotelBookingUseCase, HttpContext.RequestAborted);
            return Created(nameof(PostBookingAsync), createdHotelBooking);
        }
    }
}
