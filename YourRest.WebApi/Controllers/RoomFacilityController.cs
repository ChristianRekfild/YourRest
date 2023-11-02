using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces.Facility;

namespace YourRest.WebApi.Controllers
{
    [Route("api/roomfacilities")]
    [ApiController]
    [FluentValidationAutoValidation]
    public class RoomFacilityController : ControllerBase
    {
        private readonly IEditRoomFacilityUseCase editRoomFacilityUseCase;
        private readonly IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase;
        private readonly IRemoveRoomFacilityUseCase removeRoomFacilityUseCase;
        private readonly IGetAllRoomFacilitiesUseCase getAllRoomFacilitiesUseCase;

        public RoomFacilityController(
            IEditRoomFacilityUseCase editRoomFacilityUseCase,
            IGetRoomFacilityByIdUseCase getRoomFacilityByIdUseCase,
            IRemoveRoomFacilityUseCase removeRoomFacilityUseCase,
            IGetAllRoomFacilitiesUseCase getAllRoomFacilitiesUseCase)
        {
            this.editRoomFacilityUseCase = editRoomFacilityUseCase;
            this.getRoomFacilityByIdUseCase = getRoomFacilityByIdUseCase;
            this.removeRoomFacilityUseCase = removeRoomFacilityUseCase;
            this.getAllRoomFacilitiesUseCase = getAllRoomFacilitiesUseCase;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoomFacilities()
        {
            return Ok(await getAllRoomFacilitiesUseCase.ExecuteAsync(HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRoomFacilityById([FromRoute] RouteViewModel route)
        {
            return Ok(await getRoomFacilityByIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveRoomFacilityById([FromRoute] RouteViewModel route)
        {
            await removeRoomFacilityUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted);
            return Ok($"RoomFacility id:{route.Id} has been removed from the current room");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditRoomFacility([FromRoute] RouteViewModel route, [FromBody] RoomFacilityDto roomFacility)
        {
            await editRoomFacilityUseCase.ExecuteAsync(route.Id, roomFacility, HttpContext.RequestAborted);
            return Ok($"RoomFacility id:{route.Id} has been successfully changed in the current issue");
        }
    }
}
