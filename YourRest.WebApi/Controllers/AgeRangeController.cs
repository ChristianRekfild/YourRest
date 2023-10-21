using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces.Age;
using YourRest.Application.UseCases.Room;
using YourRest.Domain.Entities;

namespace YourRest.WebApi.Controllers
{

    [ApiController]
    [Route("api/operator/AgeRange/")]
    public class AgeRangeController : ControllerBase
    {
        private readonly ICreateAgeRangeUseCase _createAgeRangeUseCase;
        private readonly IGetAgeRangeByIdUseCase _getAgeRangeByIdUseCase;
        private readonly IEditAgeRangeUseCase _editAgeRangeUseCase;

        public AgeRangeController(ICreateAgeRangeUseCase createAgeRangeUseCase, IGetAgeRangeByIdUseCase getAgeRangeByIdUseCase, IEditAgeRangeUseCase editAgeRangeUseCase)
        {
            this._createAgeRangeUseCase = createAgeRangeUseCase;
            this._getAgeRangeByIdUseCase = getAgeRangeByIdUseCase;
            this._editAgeRangeUseCase = editAgeRangeUseCase;
        }

        [HttpGet]
        [Route("{ageRangeId}")]

        public async Task<IActionResult> GetAgeRange(int ageRangeId)
        {
            var ageRangeResponse = await _getAgeRangeByIdUseCase.ExecuteAsync(ageRangeId);
            return Ok(ageRangeResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostAgeRange([FromBody] AgeRangeDto ageRangeDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var createdRoom = await _createAgeRangeUseCase.ExecuteAsync(ageRangeDto);
            return CreatedAtAction(nameof(PostAgeRange), createdRoom);
        }

        [HttpPut]
        public async Task<IActionResult> EditAgeRange([FromBody] AgeRangeWithIdDto ageRangeWithId)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            await _editAgeRangeUseCase.ExecuteAsync(ageRangeWithId);
            return Ok("The AgeRange has been edited");

        }
    }


}
