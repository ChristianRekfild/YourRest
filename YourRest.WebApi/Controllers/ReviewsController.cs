using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.Security.Claims;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operators/reviews")]
    [FluentValidationAutoValidation]
    public class ReviewsController : ControllerBase
    {
        private readonly ICreateReviewUseCase _useCase;

        public ReviewsController(ICreateReviewUseCase useCase)
        {
            _useCase = useCase;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReviewDto reviewDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = HttpContext.User;
            var identity = user.Identity as ClaimsIdentity;
            var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sub == null)
            {
                return NotFound("User not found");
            }

            var createdReview = await _useCase.Execute(reviewDto, sub);

            return CreatedAtAction(nameof(Post), createdReview);
        }
    }
}