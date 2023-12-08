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
    //[Route("api/bookings/")]
    [FluentValidationAutoValidation]
    public class BookingController : Controller
    {
        private readonly ICreateBookingUseCase _createHotelBookingUseCase;
        private readonly IGetBookingDatesByRoomIdUseCase _getBookingDatesByRoomIdUseCase;
        private readonly IGetRoomsByCityAndBookingDatesUseCase _getRoomsByCityAndBookingDatesUseCase;
        private readonly IGetRoomsByAccommodationAndBookingDatesUseCase _getRoomsByAccommodationAndBookingDatesUseCase;



        public BookingController(
            ICreateBookingUseCase createHotelBookingUseCase, 
            IGetBookingDatesByRoomIdUseCase getBookingDatesByRoomIdUseCase,
            IGetRoomsByCityAndBookingDatesUseCase getRoomsByCityAndBookingDatesUseCase,
            IGetRoomsByAccommodationAndBookingDatesUseCase getRoomsByAccommodationAndBookingDatesUseCase)
        {
            _createHotelBookingUseCase = createHotelBookingUseCase;
            _getBookingDatesByRoomIdUseCase = getBookingDatesByRoomIdUseCase;
            _getRoomsByCityAndBookingDatesUseCase = getRoomsByCityAndBookingDatesUseCase;
            _getRoomsByAccommodationAndBookingDatesUseCase = getRoomsByAccommodationAndBookingDatesUseCase;
        }

        [HttpPost]
        [Route("api/bookings/")]
        public async Task<IActionResult> PostBookingAsync([FromBody] BookingDto createHotelBookingUseCase)
        {
            var createdHotelBooking = await _createHotelBookingUseCase.ExecuteAsync(createHotelBookingUseCase, HttpContext.RequestAborted);
            return Created(nameof(PostBookingAsync), createdHotelBooking);
        }

        [HttpGet]
        [Route("api/rooms/{roomId}/bookings/dates")]
        public async Task<IActionResult> GetBookingDateByRoomIdAsync(int roomId)
        {
            var occupiedDateList = await _getBookingDatesByRoomIdUseCase.ExecuteAsync(roomId, HttpContext.RequestAborted);
            return Ok(occupiedDateList);
        }

        [HttpGet]
        [Route("api/cities/{cityId}/rooms/{startDate}/{endDate}")]
        public async Task<IActionResult> GetRoomsByCityAndBookingDatesUseCase(string startDate, string endDate, int cityId)
        {
            var roomList = await _getRoomsByCityAndBookingDatesUseCase.ExecuteAsync(DateOnly.Parse(startDate), DateOnly.Parse(endDate), cityId, HttpContext.RequestAborted);
            return Ok(roomList);
        }

        [HttpGet]
        [Route("api/accommodations/{accommodationId}/rooms/{startDate}/{endDate}")]
        public async Task<IActionResult> GetRoomsByAccommodationAndBookingDatesUseCase(string startDate, string endDate, int accommodationId)
        {
            var roomList = await _getRoomsByAccommodationAndBookingDatesUseCase.ExecuteAsync(DateOnly.Parse(startDate), DateOnly.Parse(endDate), accommodationId, HttpContext.RequestAborted);
            return Ok(roomList);
        }
    }
}
