using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.AccommodationFacility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class AddAccommodationFacilityUseCase : IAddAccommodationFacilityUseCase
    {
        private readonly IAccommodationFacilityRepository _accommodationFacilityRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public AddAccommodationFacilityUseCase(
            IAccommodationFacilityRepository accommodationFacilityRepository,
            IAccommodationRepository accommodationRepository)
        {
            _accommodationFacilityRepository = accommodationFacilityRepository;
            _accommodationRepository = accommodationRepository;
        }

        public async Task ExecuteAsync(int accommodationId, AccommodationFacilityIdDto accommodationFacilityDto,
            CancellationToken cancellationToken)
        {
            var accommodations = await _accommodationRepository.GetWithIncludeAsync(
                a => a.Id == accommodationId,
                cancellationToken,
                a => a.AccommodationFacilities
            );
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {accommodationId} not found");
            }

            var facilityId = accommodationFacilityDto.FacilityId;

            if (accommodation.AccommodationFacilities.Any(f => f.AccommodationFacilityId == facilityId))
            {
                throw new ValidationException(
                    $"Facility with id {facilityId} is already linked to this accommodation.");
            }

            var facility = await _accommodationFacilityRepository.GetAsync(facilityId, cancellationToken);

            if (facility == null)
            {
                throw new EntityNotFoundException($"AccommodationFacility with id {facilityId} not found");
            }

            accommodation.AccommodationFacilities.Add(new AccommodationFacilityLink
            {
                AccommodationFacility = facility,
                Accommodation = accommodation,
            });

            await _accommodationRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
