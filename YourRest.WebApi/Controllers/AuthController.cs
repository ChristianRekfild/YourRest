using YourRest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Services;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
   
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;



        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("api/token")]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsViewModel credentials)
        {
            var tokenResponse = await _authenticationService.AuthenticateAsync(credentials.Username, credentials.Password);
            return Ok(tokenResponse);
        }
    }
}