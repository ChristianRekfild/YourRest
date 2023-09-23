using HotelManagementWebApi.Application.UseCase.Reviews.CustomException;
using HotelManagementWebApi.Application.UseCase.Reviews.Dto;
using HotelManagementWebApi.Domain.Entities;
using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Domain.ValueObjects.Reviews;

namespace HotelManagementWebApi.Application.UseCase.Reviews
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
            var comment = new Comment(reviewDto.Comment);
            var rating = new Rating(reviewDto.Rating);

            if (booking == null)
            {
                throw new BookingNotFoundException("Booking not found");
            }

            var review = new Review
            {
                Booking = booking,
                Rating = rating,
                Comment = comment
            };

            var savedReview = await _reviewRepository.AddAsync(review, false);
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