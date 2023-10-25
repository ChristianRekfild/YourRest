using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces.Age;
using YourRest.Application.UseCases.Room;
using YourRest.Domain.Entities;

namespace YourRest.WebApi.Controllers
{

    [ApiController]
    [FluentValidationAutoValidation]
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
        [Route("api/operator/AgeRange/{id}")]
        public async Task<IActionResult> GetAgeRange(int id)
        {
            var ageRangeResponse = await _getAgeRangeByIdUseCase.ExecuteAsync(id);
            return Ok(ageRangeResponse);
        }

        [HttpPost]
        [Route("api/operator/AgeRange/")]
        public async Task<IActionResult> PostAgeRange([FromBody] AgeRangeDto ageRangeDto)
        {
            var createdRoom = await _createAgeRangeUseCase.ExecuteAsync(ageRangeDto);
            return CreatedAtAction(nameof(PostAgeRange), createdRoom);
        }

        [HttpPut]
        [Route("api/operator/AgeRange/")]
        public async Task<IActionResult> EditAgeRange([FromBody] AgeRangeWithIdDto ageRangeWithId)
        {
            await _editAgeRangeUseCase.ExecuteAsync(ageRangeWithId);
            return Ok("The AgeRange has been edited");
        }
    }
}
