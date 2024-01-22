using YourRest.Application.Dto;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class CreateAccommodationUseCase : ICreateAccommodationUseCase
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAccommodationRepository _userAccommodationRepository;

        public CreateAccommodationUseCase(
            IAccommodationTypeRepository accommodationTypeRepository,
            IAccommodationRepository accommodationRepository,
            IUserRepository userRepository,
            IUserAccommodationRepository userAccommodationRepository
        ) {
            _accommodationTypeRepository = accommodationTypeRepository;
            _accommodationRepository = accommodationRepository;
            _userRepository = userRepository;
            _userAccommodationRepository = userAccommodationRepository;
        }

        public async Task<Dto.AccommodationDto> ExecuteAsync(CreateAccommodationDto accommodationDto, string userKeyCloakId, CancellationToken cancellationToken)
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

            var accommodation = new Infrastructure.Core.Contracts.Models.AccommodationDto();
            accommodation.Description = accommodationDto.Description;
            accommodation.Name = accommodationDto.Name;
            accommodation.AccommodationTypeId = accommodationDto.AccommodationTypeId;
            
            if (accommodationDto.Stars >= 1 && accommodationDto.Stars <= 5)
            {
                var accommodationStarRating = new AccommodationStarRatingDto
                {
                    Stars = accommodationDto.Stars.Value, 
                    Accommodation = accommodation
                };
                accommodation.StarRating = accommodationStarRating;
            }

            accommodation = await _accommodationRepository.AddAsync(accommodation, cancellationToken: cancellationToken);

            var userAccommodationDto = await _userAccommodationRepository.AddAsync(new UserAccommodationDto { User = user, Accommodation = accommodation });

            var accommodationTypeDto = new Dto.AccommodationTypeDto()
            {
                Id = accommodationType.Id,
                Name = accommodationType.Name
            };
            var savedAccommodationDto = new Dto.AccommodationDto()
            {
                Id = accommodation.Id,
                Description = accommodation.Description,
                Name = accommodation.Name,
                AccommodationType = accommodationTypeDto,
                Stars = accommodation.StarRating?.Stars
            };

            return savedAccommodationDto;
        }
    }
}