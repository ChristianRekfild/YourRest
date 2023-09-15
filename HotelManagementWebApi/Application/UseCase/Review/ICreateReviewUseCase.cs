using HotelManagementWebApi.Application.UseCase.Review.Dto;

namespace HotelManagementWebApi.Application.UseCase.Review
{
    public interface ICreateReviewUseCase
    {
        Task<SavedReviewDto> Execute(ReviewDto reviewDto);
    }
}
