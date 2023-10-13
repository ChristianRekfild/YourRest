using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.Facility;
using YourRest.Application.Interfaces.Room;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IEditRoomUseCase editRoomUseCase;
        private readonly IGetRoomByIdUseCase getRoomByIdUseCase;
        private readonly IRemoveRoomUseCase removeRoomUseCase;
        private readonly IGetFacilitiesByRoomIdUseCase getFacilitiesByRoomIdUseCase;
        private readonly IAddRoomFacilityUseCase addRoomFacilityUseCase;
        private readonly IGetRoomListUseCase _getRoomListUseCase;
        private readonly ICreateRoomUseCase _createtRoomUseCase;

        public RoomController(
            IEditRoomUseCase editRoomUseCase,
            IGetRoomByIdUseCase getRoomByIdUseCase,
            IRemoveRoomUseCase removeRoomUseCase,
            IGetFacilitiesByRoomIdUseCase getFacilitiesByRoomIdUseCase,
            IAddRoomFacilityUseCase addRoomFacilityUseCase,
            IGetRoomListUseCase getRoomListUseCase,
            ICreateRoomUseCase createtRoomUseCase)
        {
            this.editRoomUseCase = editRoomUseCase;
            this.getRoomByIdUseCase = getRoomByIdUseCase;
            this.removeRoomUseCase = removeRoomUseCase;
            this.getFacilitiesByRoomIdUseCase = getFacilitiesByRoomIdUseCase;
            this.addRoomFacilityUseCase = addRoomFacilityUseCase;
            _getRoomListUseCase = getRoomListUseCase;
            _createtRoomUseCase = createtRoomUseCase;
        }

        [HttpGet]
        [Route("accommodations/{accommodationId}")]
        public async Task<IActionResult> GetAllRooms(int accommodationId)
        {
            var regions = await _getRoomListUseCase.Execute(accommodationId);
            return Ok(regions);
        }
        [HttpPost]
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                return Ok(await getRoomByIdUseCase.ExecuteAsync(id));
            }
            catch (RoomNotFoundExeption ex)
            {
                return Problem(detail: ex.Message, statusCode: 404);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditRoom([FromBody] RoomViewModel room)
        {
            try
            {
                await editRoomUseCase.ExecuteAsync(room);
                return Ok("The room has been edited");
            }
            catch (RoomNotFoundExeption ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveRoom(int id)
        {
            try
            {
                await removeRoomUseCase.ExecuteAsync(id);
                return Ok("The room has been removed");
            }
            catch (RoomNotFoundExeption ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpGet]
        [Route("{id}/facilities")]
        public async Task<IActionResult> GetFacilitiesByRoomId([FromRoute] int id)
        {
            try
            {
                return Ok(await getFacilitiesByRoomIdUseCase.ExecuteAsync(id));
            }
            catch (RoomNotFoundExeption ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpPost]
        [Route("{id}/facilities")]
        public async Task<IActionResult> AddFacilityToRoom([FromRoute] int id, [FromBody] RoomFacilityViewModel roomFacility)
        {
            try
            {
                roomFacility.RoomId = id;
                await addRoomFacilityUseCase.ExecuteAsync(roomFacility);
                return Ok("The room facility has been added to current room");
            }
            catch (RoomNotFoundExeption ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (RoomFacilityInProcessException ex) { return Problem(detail: ex.Message, statusCode: 422); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

    }
}