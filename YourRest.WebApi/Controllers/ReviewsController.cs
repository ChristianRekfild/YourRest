using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.CustomErrors;

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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReviewDto reviewDto)
        {
            try
            {
                var createdReview = await _useCase.Execute(reviewDto);

                return CreatedAtAction(nameof(Post), createdReview);
            }
            catch (BookingNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}