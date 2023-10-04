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
        [Route("api/rooms/{accomodationId}")]
        public async Task<IActionResult> GetAllRooms(int accomodationId)
        {
            var regions = await _getRoomListUseCase.Execute(accomodationId);
            return Ok(regions);
        }
        [HttpPost]
        [Route("api/rooms")]
        public async Task<IActionResult> Post([FromBody] RoomDto roomDto)
        {
            try
            {
                var createdRoom = await _createtRoomUseCase.Execute(roomDto);

                return CreatedAtAction(nameof(Post), createdRoom);
            }
            catch (RoomNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}