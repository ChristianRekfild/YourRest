using Microsoft.AspNetCore.Mvc;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace YourRest.WebApi.Controllers
{
    [ApiController]
    [Route("api/operator/review")]
    [FluentValidationAutoValidation]
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
            var createdReview = await _useCase.Execute(reviewDto);

            return CreatedAtAction(nameof(Post), createdReview);
        }
    }
}