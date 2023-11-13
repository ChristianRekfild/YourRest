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

        public async Task<AccommodationDto> Execute(CreateAccommodationDto accommodationDto, CancellationToken cancellationToken)
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

            var savedAccommodation = await _accommodationRepository.AddAsync(accommodation, cancellationToken:cancellationToken);

            var AccommodationTypeDto = new AccommodationTypeDto()
            {
                Id = accommodationType.Id,
                Name = accommodationType.Name
            };
            var savedAccommodationDto = new AccommodationDto()
            {
                Id = savedAccommodation.Id,
                Description = savedAccommodation.Description,
                Name = savedAccommodation.Name,
                AccommodationType = AccommodationTypeDto,
            };

            return savedAccommodationDto;
        }
    }
}