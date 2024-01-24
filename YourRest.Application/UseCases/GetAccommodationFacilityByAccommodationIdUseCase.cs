using AutoMapper;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.AccommodationFacility;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetAccommodationFacilityByAccommodationIdUseCase : IGetAccommodationFacilityByAccommodationIdUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;

        public GetAccommodationFacilityByAccommodationIdUseCase(IAccommodationRepository accommodationRepository, IMapper mapper)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccommodationFacilityWithIdDto>> ExecuteAsync(int accommodationId, CancellationToken cancellationToken)
        {
            var accommodations = await _accommodationRepository.GetAccommodationsWithFacilitiesAsync(accommodationId, cancellationToken);
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id number {accommodationId} not found");
            }
            if (!accommodation.AccommodationFacilities.Any())
            {
                throw new EntityNotFoundException($"Not found AccommodationFacility in current accommodation (id : {accommodationId})");
            }
            var facilities = accommodation.AccommodationFacilities
                .Select(link => link.AccommodationFacility);
            
            var facilitiesDto = _mapper.Map<IEnumerable<AccommodationFacilityWithIdDto>>(facilities);

            return facilitiesDto;
        }
    }
}
