using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
   
    public class RoomTypeController : ControllerBase
    {

        private readonly IGetRoomTypeListUseCase _getRoomTypeListUseCase;



        public RoomTypeController(IGetRoomTypeListUseCase getRoomTypeListUseCase)
        {
            _getRoomTypeListUseCase = getRoomTypeListUseCase;
        }

        [HttpGet]
        [Route("api/roomTypes")]
        public async Task<IActionResult> GetAllRoomTypes()
        {
            var types = await _getRoomTypeListUseCase.Execute();
            return Ok(types);
        }
    }
}