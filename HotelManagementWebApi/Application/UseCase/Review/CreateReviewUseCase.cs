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
        private readonly IReviewRepository _repository;
        private readonly IBookingRepository _bookingRepository;

        public CreateReviewUseCase(IReviewRepository repository, IBookingRepository bookingRepository)
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
        }

        public async Task<SavedReviewDto> Execute(ReviewDto reviewDto)
        {
            var booking = await _bookingRepository.FindAsync(reviewDto.BookingId);
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

            var savedReview = await _repository.SaveReviewAsync(review);

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