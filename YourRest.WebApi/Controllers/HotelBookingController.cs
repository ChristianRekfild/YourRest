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
        private readonly IGetRoomsByHotelAndBookingDatesUseCase _getRoomsByHotelAndBookingDatesUseCase;


        public BookingController(ICreateBookingUseCase createHotelBookingUseCase,
            IGetBookingDatesByRoomIdUseCase getBookingDatesByRoomIdUseCase,
            IGetRoomsByCityAndBookingDatesUseCase getRoomsByCityAndBookingDatesUseCase,
            IGetRoomsByHotelAndBookingDatesUseCase getRoomsByHotelAndBookingDatesUseCase
            )
        {
            _createHotelBookingUseCase = createHotelBookingUseCase;
            _getBookingDatesByRoomIdUseCase = getBookingDatesByRoomIdUseCase;
            _getRoomsByCityAndBookingDatesUseCase = getRoomsByCityAndBookingDatesUseCase;
            _getRoomsByHotelAndBookingDatesUseCase = getRoomsByHotelAndBookingDatesUseCase;
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
        [Route("api/citys/{cityId}/rooms/{startDate}/{endDate}")]
        public async Task<IActionResult> GetRoomsByCityAndBookingDatesUseCase(DateOnly startDate, DateOnly endDate, int cityId)
        {
            var occupiedDateList = await _getRoomsByCityAndBookingDatesUseCase.ExecuteAsync(startDate, endDate, cityId, HttpContext.RequestAborted);
            return Ok(occupiedDateList);
        }

        [HttpGet]
        [Route("api/accommodations/{accommodationId}/rooms/{startDate}/{endDate}")]
        public async Task<IActionResult> GetRoomsByHotelIdAndBookingDatesUseCase(DateOnly startDate, DateOnly endDate, int accommodationId)
        {
            var occupiedDateList = await _getRoomsByHotelAndBookingDatesUseCase.ExecuteAsync(startDate, endDate, accommodationId, HttpContext.RequestAborted);
            return Ok(occupiedDateList);
        }
    }
}
