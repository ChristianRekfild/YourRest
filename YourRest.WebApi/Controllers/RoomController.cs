using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.Facility;
using YourRest.Application.Interfaces.Room;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [FluentValidationAutoValidation]
    public class RoomController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEditRoomUseCase editRoomUseCase;
        private readonly IGetRoomByIdUseCase getRoomByIdUseCase;
        private readonly IRemoveRoomUseCase removeRoomUseCase;
        private readonly IGetFacilitiesByRoomIdUseCase getFacilitiesByRoomIdUseCase;
        private readonly IAddRoomFacilityUseCase addRoomFacilityUseCase;
        private readonly IGetRoomListUseCase _getRoomListUseCase;
        private readonly ICreateRoomUseCase _createtRoomUseCase;

        public RoomController(
            IMapper mapper,
            IEditRoomUseCase editRoomUseCase,
            IGetRoomByIdUseCase getRoomByIdUseCase,
            IRemoveRoomUseCase removeRoomUseCase,
            IGetFacilitiesByRoomIdUseCase getFacilitiesByRoomIdUseCase,
            IAddRoomFacilityUseCase addRoomFacilityUseCase,
            IGetRoomListUseCase getRoomListUseCase,
            ICreateRoomUseCase createtRoomUseCase)
        {
            this.mapper = mapper;
            this.editRoomUseCase = editRoomUseCase;
            this.getRoomByIdUseCase = getRoomByIdUseCase;
            this.removeRoomUseCase = removeRoomUseCase;
            this.getFacilitiesByRoomIdUseCase = getFacilitiesByRoomIdUseCase;
            this.addRoomFacilityUseCase = addRoomFacilityUseCase;
            _getRoomListUseCase = getRoomListUseCase;
            _createtRoomUseCase = createtRoomUseCase;
        }

        [HttpGet]
        [Route("api/accommodations/{accommodationId}/rooms")]
        public async Task<IActionResult> GetAllRooms(int accommodationId)
        {
            var regions = await _getRoomListUseCase.Execute(accommodationId, HttpContext.RequestAborted);
            return Ok(regions);
        }

        [HttpPost]
        [Route("api/accommodations/{accommodationId}/rooms")]
        public async Task<IActionResult> Post([FromRoute] int accommodationId, [FromBody] RoomDto roomDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRoom = await _createtRoomUseCase.Execute(roomDto, accommodationId, HttpContext.RequestAborted);
            return CreatedAtAction(nameof(Post), createdRoom);
        }

        [HttpGet]
        [Route("api/rooms/{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] RouteViewModel route)
        {
            return Ok(await getRoomByIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [HttpPut]
        [Route("api/accommodations/{accommodationId}/rooms/{id}")]
        public async Task<IActionResult> EditRoom([FromRoute] int accommodationId, [FromRoute] RouteViewModel route, [FromBody] RoomDto room)
        {
            var roomWithId = mapper.Map<RoomWithIdDto>(room);
            roomWithId.Id = route.Id;
            await editRoomUseCase.ExecuteAsync(roomWithId, accommodationId, HttpContext.RequestAborted);
            return Ok("The room has been edited");
        }

        [HttpDelete]
        [Route("api/rooms/{id}")]
        public async Task<IActionResult> RemoveRoom([FromRoute] RouteViewModel route)
        {
            await removeRoomUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted);
            return Ok("The room has been removed");
        }

        [HttpGet]
        [Route("api/rooms/{id}/facilities")]
        public async Task<IActionResult> GetFacilitiesByRoomId([FromRoute] RouteViewModel route)
        {
            return Ok(await getFacilitiesByRoomIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [HttpPost]
        [Route("api/rooms/{id}/facilities")]
        public async Task<IActionResult> AddFacilityToRoom([FromRoute] RouteViewModel route, [FromBody] IEnumerable<RoomFacilityDto> roomFacility)
        {
            await addRoomFacilityUseCase.ExecuteAsync(route.Id, roomFacility, HttpContext.RequestAborted);
            return Ok("The room facility has been added to current room");
        }
    }
}
