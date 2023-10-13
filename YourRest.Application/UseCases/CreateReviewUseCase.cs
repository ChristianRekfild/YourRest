using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.ValueObjects.Reviews;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class CreateReviewUseCase : ICreateReviewUseCase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public CreateReviewUseCase(IReviewRepository reviewRepository, IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        public async Task<SavedReviewDto> Execute(ReviewDto reviewDto, string userKeyCloakId)
        {
            var users = await _userRepository.FindAsync(a => a.KeyCloakId == userKeyCloakId);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new UserNotFoundException(userKeyCloakId);
            }

            var booking = await _bookingRepository.GetAsync(reviewDto.BookingId);
            var comment = reviewDto.Comment;
            var rating = RatingVO.Create(reviewDto.Rating);

            if (booking == null)
            {
                throw new BookingNotFoundException("Booking not found");
            }

            if (user.AccommodationId != booking.AccommodationId)
            {
                throw new UserReviewCreationException(user.Id, booking.Id);
            }

            var review = new Review
            {
                Booking = booking,
                Rating = rating,
                Comment = comment,
                User = user
            };

            var savedReview = await _reviewRepository.AddAsync(review, false);
            await _reviewRepository.SaveChangesAsync();

            var savedReviewDto = new SavedReviewDto 
            {
                Id = savedReview.Id,
                BookingId = savedReview.Booking.Id,
                Comment = savedReview.Comment,
                Rating = savedReview.Rating.Value
            };

            return savedReviewDto;
        }
    }
}