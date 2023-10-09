using Microsoft.AspNetCore.Mvc;
using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;

namespace YourRest.WebApi.Controllers
{
    [Route("api/operator/{operatorId}/accommodation/{accommodationId}/room/{roomId}")]
    [ApiController]
    public class RoomFacilityController : ControllerBase
    {
        private readonly IAddRoomFacilityUseCase addRoomFacilityUseCase;
        private readonly IEditRoomFacilityUseCase editRoomFacilityUseCase;
        private readonly IGetRoomFacilitiesByRoomIdUseCase getRoomFacilitiesByRoomIdUseCase;
        private readonly IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase;
        private readonly IRemoveRoomFacilityUseCase removeRoomFacilityUseCase;
        public RoomFacilityController(
            IAddRoomFacilityUseCase addRoomFacilityUseCase,
            IEditRoomFacilityUseCase editRoomFacilityUseCase,
            IGetRoomFacilitiesByRoomIdUseCase getRoomFacilitiesByRoomIdUseCase,
            IGetRoomFacilityByIdUseCase getRoomFacilityByIdUse,
            IRemoveRoomFacilityUseCase  removeRoomFacilityUseCase)
        {
            this.addRoomFacilityUseCase = addRoomFacilityUseCase;
            this.editRoomFacilityUseCase = editRoomFacilityUseCase;
            this.getRoomFacilitiesByRoomIdUseCase = getRoomFacilitiesByRoomIdUseCase;
            this.getRoomFacilityByIdUseCase = getRoomFacilityByIdUse;
            this.removeRoomFacilityUseCase = removeRoomFacilityUseCase;
        }
        [HttpGet]
        [Route("facilities")]
        public async Task<IActionResult> GetRoomFacilitiesById(int roomId)
        {
            try
            {
                return Ok(await getRoomFacilitiesByRoomIdUseCase.ExecuteAsync(roomId));
            }
            catch (RoomNotFoundExeption ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 422); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpGet]
        [Route("facility/{id}")]
        public async Task<IActionResult> GetRoomFacilityById(int id)
        {
            try
            {
                return Ok(await getRoomFacilityByIdUseCase.ExecuteAsync(id));
            }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpPost]
        [Route("facility/remove")]
        public async Task<IActionResult> RemoveRoomFacilityById([FromBody] RoomFacilityViewModel roomFacility)
        {
            try
            {
                await removeRoomFacilityUseCase.ExecuteAsync(roomFacility);
                return Ok($"RoomFacility id:{roomFacility.Id} has been removed from the current room");
            }
            catch (RoomFacilityNotFoundException ex) { return Problem(detail: ex.Message, statusCode: 404); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpPost]
        [Route("facility/add")]
        public async Task<IActionResult> AddRoomFacility([FromBody] RoomFacilityViewModel roomFacility)
        {
            try
            {
                await addRoomFacilityUseCase.ExecuteAsync(roomFacility);
                return Ok($"Service \"{roomFacility.Name}\" has been added to the current room");
            }
            catch (RoomAlreadyExistsException ex) { return Problem(detail: ex.Message, statusCode: 422); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
        [HttpPut]
        [Route("facility/edit")]
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
