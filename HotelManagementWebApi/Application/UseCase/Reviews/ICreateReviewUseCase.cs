using HotelManagementWebApi.Application.UseCase.Reviews.Dto;

namespace HotelManagementWebApi.Application.UseCase.Reviews
{
    public interface ICreateReviewUseCase
    {
        Task<SavedReviewDto> Execute(ReviewDto reviewDto);
    }
}
