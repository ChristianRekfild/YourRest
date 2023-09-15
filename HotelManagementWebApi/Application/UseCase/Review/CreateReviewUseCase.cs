using HotelManagementWebApi.Application.UseCase.Review.Dto;
using CommentVO = HotelManagementWebApi.Domain.ValueObjects.Review.Comment;
using RatingVO = HotelManagementWebApi.Domain.ValueObjects.Review.Rating;
using HotelManagementWebApi.Domain.ValueObjects.Booking;
using HotelManagementWebApi.Domain.Repositories;
using ReviewEntity = HotelManagementWebApi.Domain.Entities.Review.Review;
using HotelManagementWebApi.Application.UseCase.Review.CustomException;

namespace HotelManagementWebApi.Application.UseCase.Review
{
    public class CreateReviewUseCase : ICreateReviewUseCase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookingRepository _bookingRepository;

        public CreateReviewUseCase(IReviewRepository reviewRepository, IBookingRepository bookingRepository)
        {
            _reviewRepository = reviewRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<SavedReviewDto> Execute(ReviewDto reviewDto)
        {
            var booking = await _bookingRepository.GetAsync(reviewDto.BookingId);
            var comment = new CommentVO(reviewDto.Comment);
            var rating = new RatingVO(reviewDto.Rating);

            if (booking == null)
            {
                throw new BookingNotFoundException("Booking not found");
            }

            var review = new ReviewEntity
            {
                Booking = booking,
                Rating = rating,
                Comment = comment
            };

            var savedReview = _reviewRepository.Add(review);
            await _reviewRepository.SaveChangesAsync();

            var savedReviewDto = new SavedReviewDto 
            {
                Id = savedReview.Id,
                BookingId = savedReview.Booking.Id,
                Comment = savedReview.Comment.Value,
                Rating = savedReview.Rating.Value
            };

            return savedReviewDto;
        }
    }
}