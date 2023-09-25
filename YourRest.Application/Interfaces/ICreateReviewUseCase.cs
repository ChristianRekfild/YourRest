using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateReviewUseCase
    {
        Task<SavedReviewDto> Execute(ReviewDto reviewDto);
    }
}
