using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.AccommodationFacility;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class RemoveAccommodationFacilityUseCase : IRemoveAccommodationFacilityUseCase
    {
        private readonly IAccommodationFacilityRepository _accommodationFacilityRepository;

        public RemoveAccommodationFacilityUseCase(
            IAccommodationFacilityRepository accommodationFacilityRepository
        ) {
            _accommodationFacilityRepository = accommodationFacilityRepository;
        }

        public async Task ExecuteAsync(int accommodationId, int facilityId, CancellationToken cancellationToken)
        {
            var facilities = await _accommodationFacilityRepository.GetWithIncludeAsync(
                f => f.Id == facilityId,
                cancellationToken,
                f => f.AccommodationFacilities
            );
            var facility = facilities.FirstOrDefault();

            if (facility == null)
            {
                throw new EntityNotFoundException($"AccommodationFacility with id {facilityId} not found");
            }

            var linkToRemove = facility.AccommodationFacilities.FirstOrDefault(link => link.AccommodationId == accommodationId);

            if (linkToRemove == null)
            {
                throw new EntityNotFoundException($"Link between Accommodation {accommodationId} and Facility {facilityId} not found");
            }

            facility.AccommodationFacilities.Remove(linkToRemove);

            await _accommodationFacilityRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
