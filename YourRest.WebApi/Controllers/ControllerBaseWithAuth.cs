using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace YourRest.WebApi.Controllers
{
    public abstract class ControllerBaseWithAuth : ControllerBase
    {
        protected IActionResult CheckClaims()
        {
            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }
            return Ok(sub);
        }
    }
}
