using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Application.Services;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IGetUserInfoUseCase _getUserInfoUseCase;

        public AuthController(IAuthenticationService authenticationService, IGetUserInfoUseCase getUserInfoUseCase)
        {
            _authenticationService = authenticationService;
            _getUserInfoUseCase = getUserInfoUseCase;
        }

        [HttpPost]
        [Route("api/token")]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsViewModel credentials)
        {
            var tokenResponse = await _authenticationService.AuthenticateAsync(credentials.Username, credentials.Password);
            return Ok(tokenResponse);
        }
        
        [Authorize]
        [HttpGet]
        [Route("api/user")]
        public async Task<IActionResult> GetUserDetailsAsync()
        {
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }

            var userDetails = await _getUserInfoUseCase.ExecuteAsync(sub, HttpContext.RequestAborted);

            return Ok(userDetails);
        }
    }
}