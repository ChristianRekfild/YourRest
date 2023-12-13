using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;

namespace YourRest.Application.UseCases
{
    public class CreateAccommodationUseCase : ICreateAccommodationUseCase
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public CreateAccommodationUseCase(IAccommodationTypeRepository accommodationTypeRepository, IAccommodationRepository accommodationRepository)
        {
            _accommodationTypeRepository = accommodationTypeRepository;
            _accommodationRepository = accommodationRepository;
        }

        public async Task<AccommodationDto> ExecuteAsync(CreateAccommodationDto accommodationDto, CancellationToken cancellationToken)
        {
            var accommodationType = await _accommodationTypeRepository.GetAsync(accommodationDto.AccommodationTypeId, cancellationToken);

            if (accommodationType == null)
            {
                throw new EntityNotFoundException($"Accommodation Type with id {accommodationDto.AccommodationTypeId} not found");
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

            var savedAccommodation = await _accommodationRepository.AddAsync(accommodation, cancellationToken:cancellationToken);

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