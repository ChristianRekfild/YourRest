using AutoMapper;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.AccommodationFacility;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetAllAccommodationFacilitiesUseCase : IGetAllAccommodationFacilitiesUseCase
    {
        private readonly IAccommodationFacilityRepository accommodationFacilityRepository;
        private readonly IMapper mapper;

        public GetAllAccommodationFacilitiesUseCase(IAccommodationFacilityRepository accommodationFacilityRepository, IMapper mapper)
        {
            this.accommodationFacilityRepository = accommodationFacilityRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AccommodationFacilityWithIdDto>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var accommodationFacilities = await accommodationFacilityRepository.GetAllAsync(cancellationToken);
            if (!accommodationFacilities.Any())
            {
                return new List<AccommodationFacilityWithIdDto>();
            }
            return mapper.Map<IEnumerable<AccommodationFacilityWithIdDto>>(accommodationFacilities);
        }
    }
}
