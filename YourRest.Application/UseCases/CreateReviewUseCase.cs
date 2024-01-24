using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Infrastructure.Core.Contracts.ValueObjects.Reviews;

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

        public async Task<SavedReviewDto> ExecuteAsync(Dto.Models.ReviewDto reviewDto, string userKeyCloakId)
        {
            var users = await _userRepository.FindAsync(a => a.KeyCloakId == userKeyCloakId);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(userKeyCloakId);
            }

            var booking = await _bookingRepository.GetAsync(reviewDto.BookingId);
            var comment = reviewDto.Comment;
            var rating = RatingVO.Create(reviewDto.Rating);

            if (booking == null)
            {
                throw new EntityNotFoundException($"Booking {reviewDto.BookingId} not found");
            }

            //TODO
            //if (user.UserAccommodations.Any(ua => ua.AccommodationId != booking.AccommodationId))
            //{
            //    throw new ValidationException($"User with id {user.Id} cannot create a review for booking {booking.Id}.");
            //}

            var review = new Infrastructure.Core.Contracts.Models.ReviewDto
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