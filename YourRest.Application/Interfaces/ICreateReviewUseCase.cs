using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface ICreateReviewUseCase
    {
        Task<SavedReviewDto> Execute(ReviewDto reviewDto, string userKeyCloakId);
    }
}
