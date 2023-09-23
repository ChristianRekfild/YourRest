using Microsoft.AspNetCore.Mvc;
using HotelManagementWebApi.Application.UseCase.Reviews.Dto;
using HotelManagementWebApi.Application.UseCase.Reviews;
using HotelManagementWebApi.Application.UseCase.Reviews.CustomException;

namespace HotelManagementWebApi.Infrastructure.Controllers
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
            catch (BookingNotFoundException)
            {
                return NotFound();
            }
        }
    }
}