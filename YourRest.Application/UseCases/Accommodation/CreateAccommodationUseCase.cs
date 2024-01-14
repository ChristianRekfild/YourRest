using YourRest.Application.Exceptions;
using YourRest.Domain.Repositories;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces.Accommodation;

namespace YourRest.Application.UseCases.Accommodation
{
    public class CreateAccommodationUseCase : ICreateAccommodationUseCase
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUserRepository _userRepository;

        public CreateAccommodationUseCase(
            IAccommodationTypeRepository accommodationTypeRepository,
            IAccommodationRepository accommodationRepository,
            IUserRepository userRepository
        )
        {
            _accommodationTypeRepository = accommodationTypeRepository;
            _accommodationRepository = accommodationRepository;
            _userRepository = userRepository;
        }

        public async Task<AccommodationDto> ExecuteAsync(CreateAccommodationDto accommodationDto, string userKeyCloakId, CancellationToken cancellationToken)
        {
            var accommodationType = await _accommodationTypeRepository.GetAsync(accommodationDto.AccommodationTypeId, cancellationToken);

            if (accommodationType == null)
            {
                throw new EntityNotFoundException($"Accommodation Type with id {accommodationDto.AccommodationTypeId} not found");
            }

            var users = await _userRepository.FindAsync(a => a.KeyCloakId == userKeyCloakId, cancellationToken);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(userKeyCloakId);
            }

            var accommodation = new Accommodation();
            accommodation.Description = accommodationDto.Description;
            accommodation.Name = accommodationDto.Name;
            accommodation.AccommodationTypeId = accommodationDto.AccommodationTypeId;

            if (accommodationDto.Stars >= 1 && accommodationDto.Stars <= 5)
            {
                var accommodationStarRating = new AccommodationStarRating
                {
                    Stars = accommodationDto.Stars.Value,
                    Accommodation = accommodation
                };
                accommodation.StarRating = accommodationStarRating;
            }

            accommodation.UserAccommodations.Add(new UserAccommodation { User = user, Accommodation = accommodation });

            var savedAccommodation = await _accommodationRepository.AddAsync(accommodation, cancellationToken: cancellationToken);

            var accommodationTypeDto = new AccommodationTypeDto()
            {
                Id = accommodationType.Id,
                Name = accommodationType.Name
            };
            var savedAccommodationDto = new AccommodationDto()
            {
                Id = savedAccommodation.Id,
                Description = savedAccommodation.Description,
                Name = savedAccommodation.Name,
                AccommodationType = accommodationTypeDto,
                Stars = savedAccommodation.StarRating?.Stars
            };

            return savedAccommodationDto;
        }
    }
}