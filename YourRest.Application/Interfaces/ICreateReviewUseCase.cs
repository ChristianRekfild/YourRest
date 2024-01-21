using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface ICreateReviewUseCase
    {
        Task<SavedReviewDto> ExecuteAsync(ReviewDto reviewDto, string userKeyCloakId);
    }
}
