using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces.Addresses;
using YourRest.WebApi.Models;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    [FluentValidationAutoValidation]
    public class AddressController : ControllerBaseWithAuth
    {
        private readonly IAddAddressUseCase _addAddressByIdUseCase;
        private readonly IEditAddressUseCase _editAddressByIdUseCase;
        private readonly IGetAddressUseCase _getAddressUseCase;
        private readonly IGetAddressByCityIdUseCase _getAddressByCityIdUseCase;
        private readonly IGetAddressByIdUseCase _getAddressByIdUseCase;
        private readonly IRemoveAddressUseCase _removeAddressUseCase;
        private readonly IMapper _mapper;

        public AddressController(IAddAddressUseCase addAddressByIdUseCase,
            IEditAddressUseCase editAddressByIdUseCase,
            IGetAddressUseCase getAddressUseCase,
            IGetAddressByCityIdUseCase getAddressByCityIdUseCase,
            IGetAddressByIdUseCase getAddressByIdUseCase,
            IRemoveAddressUseCase removeAddressUseCase,
            IMapper mapper)
        {
            _addAddressByIdUseCase = addAddressByIdUseCase;
            _editAddressByIdUseCase = editAddressByIdUseCase;
            _getAddressUseCase = getAddressUseCase;
            _getAddressByCityIdUseCase = getAddressByCityIdUseCase;
            _getAddressByIdUseCase = getAddressByIdUseCase;
            _removeAddressUseCase = removeAddressUseCase;
            _mapper = mapper;
        }

        [HttpGet]        
        public async Task<IActionResult> GetAddresses()
        {
            return Ok(await _getAddressUseCase.ExecuteAsync(HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAddressById([FromRoute] RouteViewModel route)
        {
            return Ok(await _getAddressByIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [HttpGet]
        [Route("bycity/{id}")]
        public async Task<IActionResult> GetAddressByCityId([FromRoute] RouteViewModel route)
        {
            return Ok(await _getAddressByCityIdUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressViewModel route)
        {
            var authCheck = CheckClaims();
            if (authCheck is NotFoundObjectResult)
            { return authCheck; }

            return CreatedAtAction(nameof(CreateAddress), _mapper.Map<AddressViewModel>(await _addAddressByIdUseCase.ExecuteAsync(_mapper.Map<AddressDto>(route), HttpContext.RequestAborted)));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditAddress([FromBody] AddressViewModel route)
        {
            var authCheck = CheckClaims();
            if (authCheck is NotFoundObjectResult)
            { return authCheck; }

            return Ok(await _editAddressByIdUseCase.ExecuteAsync(_mapper.Map<AddressDto>(route), HttpContext.RequestAborted));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAddress([FromBody] RouteViewModel route)
        {
            var authCheck = CheckClaims();
            if (authCheck is NotFoundObjectResult)
            { return authCheck; }

            await _removeAddressUseCase.ExecuteAsync(route.Id, HttpContext.RequestAborted);

            return Ok();
        }
    }
}
