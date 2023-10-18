using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using YourRest.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operator/review")]
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