using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces.Facility;

namespace YourRest.WebApi.Controllers
{
    [Route("api/facilities")]
    [ApiController]
    [FluentValidationAutoValidation]
    public class RoomFacilityController : ControllerBase
    {
        private readonly IEditRoomFacilityUseCase editRoomFacilityUseCase;
        private readonly IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase;
        private readonly IRemoveRoomFacilityUseCase removeRoomFacilityUseCase;
        public RoomFacilityController(
            IEditRoomFacilityUseCase editRoomFacilityUseCase,
            IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase,
            IRemoveRoomFacilityUseCase removeRoomFacilityUseCase)
        {
            this.editRoomFacilityUseCase = editRoomFacilityUseCase;
            this.getRoomFacilityByIdUseCase = getRoomFacilityByIdUseCase;
            this.removeRoomFacilityUseCase = removeRoomFacilityUseCase;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRoomFacilityById([FromRoute] RouteViewModel route)
        {
            return Ok(await getRoomFacilityByIdUseCase.ExecuteAsync(route.Id));
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveRoomFacilityById([FromRoute] RouteViewModel route)
        {
            await removeRoomFacilityUseCase.ExecuteAsync(route.Id);
            return Ok($"RoomFacility id:{route.Id} has been removed from the current room");
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditRoomFacility([FromRoute] RouteViewModel route, [FromBody] RoomFacilityViewModel roomFacility)
        {
            roomFacility.Id = route.Id;
            await editRoomFacilityUseCase.ExecuteAsync(roomFacility);
            return Ok($"RoomFacility id:{roomFacility.Id} has been successfully changed in the current issue");
        }
    }
}
