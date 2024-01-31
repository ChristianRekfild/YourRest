using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.Application.Services;
using YourRest.Domain.Service.AccommodationService;

namespace YourRest.Application.UseCases
{
    public class CreateAccommodationUseCase : ICreateAccommodationUseCase
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUserRepository _userRepository;
        private readonly EventDispatcher _eventDispatcher;

        public CreateAccommodationUseCase(
            IAccommodationTypeRepository accommodationTypeRepository,
            IAccommodationRepository accommodationRepository,
            IUserRepository userRepository,
            EventDispatcher eventDispatcher
        ) {
            _accommodationTypeRepository = accommodationTypeRepository;
            _accommodationRepository = accommodationRepository;
            _userRepository = userRepository;
            _eventDispatcher = eventDispatcher;
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

            var accommodationBuilder = new AccommodationBuilder(accommodationDto.Name, accommodationType);
            accommodationBuilder.WithDescription(accommodationDto.Description);

            if (accommodationDto.Stars.HasValue && accommodationDto.Stars >= 1 && accommodationDto.Stars <= 5)
            {
                accommodationBuilder.WithStarRating(new AccommodationStarRating
                {
                    Stars = accommodationDto.Stars.Value
                });
            }

            var accommodation = accommodationBuilder.Build();

            accommodation.UserAccommodations.Add(new UserAccommodation { User = user, Accommodation = accommodation });

            var savedAccommodation = await _accommodationRepository.AddAsync(accommodation, cancellationToken: cancellationToken);

            foreach (var domainEvent in accommodation.ReleaseEvents())
            {
                await _eventDispatcher.Dispatch(domainEvent);
            }
            
            var accommodationTypeDto = new AccommodationTypeDto
            {
                Id = accommodationType.Id,
                Name = accommodationType.Name
            };
            var savedAccommodationDto = new AccommodationDto
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