using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;

namespace YourRest.WebApi.Controllers
{
    [Route("api/facilities")]
    [ApiController]
    public class RoomFacilityController : ControllerBase
    {
        private readonly IEditRoomFacilityUseCase editRoomFacilityUseCase;
        private readonly IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase;
        private readonly IRemoveRoomFacilityUseCase removeRoomFacilityUseCase;
        public RoomFacilityController(
            IEditRoomFacilityUseCase editRoomFacilityUseCase,
            IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase,
            IRemoveRoomFacilityUseCase  removeRoomFacilityUseCase)
        {
            this.editRoomFacilityUseCase = editRoomFacilityUseCase;
            this.getRoomFacilityByIdUseCase = getRoomFacilityByIdUseCase;
            this.removeRoomFacilityUseCase = removeRoomFacilityUseCase;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRoomFacilityById(int id)
        {
            try
            {
                return Ok(await getRoomFacilityByIdUseCase.ExecuteAsync(id));
            }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveRoomFacilityById(int id)
        {
            try
            {
                await removeRoomFacilityUseCase.ExecuteAsync(id);
                return Ok($"RoomFacility id:{id} has been removed from the current room");
            }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpPut]
        public async Task<IActionResult> EditRoomFacility([FromBody] RoomFacilityViewModel roomFacility)
        {
            try
            {
                await editRoomFacilityUseCase.ExecuteAsync(roomFacility);
                return Ok($"RoomFacility id:{roomFacility.Id} has been successfully changed in the current issue");
            }
            catch(RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (RoomAlreadyExistsException ex) { return Problem(detail: ex.Message, statusCode: 422); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }


    }
}
