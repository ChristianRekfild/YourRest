using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Room;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operator/accommodation/")]
    public class RoomController : ControllerBase
    {
        private readonly IAddRoomUseCase addRoomUseCase;
        private readonly IEditRoomUseCase editRoomUseCase;
        private readonly IGetRoomByIdUseCase getRoomByIdUseCase;
        private readonly IRemoveRoomUseCase removeRoomUseCase;

        public RoomController(
            IAddRoomUseCase addRoomUseCase,
            IEditRoomUseCase editRoomUseCase,
            IGetRoomByIdUseCase getRoomByIdUseCase,
            IRemoveRoomUseCase removeRoomUseCase)
        {
            this.addRoomUseCase = addRoomUseCase;
            this.editRoomUseCase = editRoomUseCase;
            this.getRoomByIdUseCase = getRoomByIdUseCase;
            this.removeRoomUseCase = removeRoomUseCase;
        }

        [HttpGet]
        [Route("room/{id}")]
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
        [Route("room/edit")]
        public async Task<IActionResult> EditRoom([FromBody] RoomViewModel room)
        {
            try
            {
                await editRoomUseCase.ExecuteAsync(room);
                return Ok("The room has been edited");
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
        [HttpPost]
        [Route("room/remove")]
        public async Task<IActionResult> RemoveRoom([FromBody] RoomViewModel room)
        {
            try
            {
                await removeRoomUseCase.ExecuteAsync(room);
                return Ok("The room has been removed");
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
        [HttpPost]
        [Route("room/add")]
        public async Task<IActionResult> AddRoom([FromBody] RoomViewModel room)
        {
            try
            {
                await addRoomUseCase.ExecuteAsync(room);
                return Ok("The room has been added");
            }
            catch (RoomAlreadyExistsException ex)
            {
                return Problem(detail: ex.Message, statusCode: 409);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
