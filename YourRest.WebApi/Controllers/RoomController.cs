using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.Application.UseCases;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
   
    public class RoomController : ControllerBase
    {
        private readonly IGetRoomListUseCase _getRoomListUseCase;
        private readonly ICreateRoomUseCase _createtRoomUseCase;


        public RoomController(IGetRoomListUseCase getRoomListUseCase, ICreateRoomUseCase createtRoomUseCase)
        {
            _getRoomListUseCase = getRoomListUseCase;
            _createtRoomUseCase = createtRoomUseCase;
        }

        [HttpGet]
        [Route("api/rooms/{accommodationId}")]
        public async Task<IActionResult> GetAllRooms(int accommodationId)
        {
            var regions = await _getRoomListUseCase.Execute(accommodationId);
            return Ok(regions);
        }
        [HttpPost]
        [Route("api/rooms")]
        public async Task<IActionResult> Post([FromBody] RoomDto roomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdRoom = await _createtRoomUseCase.Execute(roomDto);

                return CreatedAtAction(nameof(Post), createdRoom);

            }
            catch (AccommodationNotFoundException exception)
            {
                return UnprocessableEntity(exception.Message);
            }
        }
    }
}