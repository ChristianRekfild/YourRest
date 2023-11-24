using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Application.UseCases.HotelBookingUseCase;
using YourRest.Domain.Entities;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class BookingController : Controller
    {
        private readonly ICreateBookingUseCase _createHotelBookingUseCase;
        private readonly IGetBookingDatesByRoomIdUseCase _getBookingDatesByRoomIdUseCase;


        public BookingController(ICreateBookingUseCase createHotelBookingUseCase, IGetBookingDatesByRoomIdUseCase getBookingDatesByRoomIdUseCase)
        {
            _createHotelBookingUseCase = createHotelBookingUseCase;
            _getBookingDatesByRoomIdUseCase = getBookingDatesByRoomIdUseCase;
        }

        [HttpPost]
        [Route("api/booking/")]
        public async Task<IActionResult> PostBookingAsync([FromBody] BookingDto createHotelBookingUseCase)
        {
            var createdHotelBooking = await _createHotelBookingUseCase.ExecuteAsync(createHotelBookingUseCase, HttpContext.RequestAborted);
            return Created(nameof(PostBookingAsync), createdHotelBooking);
        }

        [HttpGet]
        [Route("api/booking/rooms/{roomId}")]
        public async Task<IActionResult> GetBookingDateByRoomIdAsync(int roomId)
        {
            var occupiedDateList = await _getBookingDatesByRoomIdUseCase.ExecuteAsync(roomId, HttpContext.RequestAborted);
            return Ok(occupiedDateList);
        }
    }
}
