using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Exceptions;
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
        private readonly IGetRoomTypeListUseCase _getRoomTypeListUseCase;



        public RoomController(IGetRoomListUseCase getRoomListUseCase, ICreateRoomUseCase createtRoomUseCase, IGetRoomTypeListUseCase getRoomTypeListUseCase)
        {
            _getRoomListUseCase = getRoomListUseCase;
            _createtRoomUseCase = createtRoomUseCase;
            _getRoomTypeListUseCase = getRoomTypeListUseCase;
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
            var createdRoom = await _createtRoomUseCase.Execute(roomDto);

            return CreatedAtAction(nameof(Post), createdRoom);
        }

        [HttpPost]
        [Route("api/rooms/roomTypes")]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var regions = await _getRoomTypeListUseCase.Execute();
            return Ok(regions);
        }
    }
}