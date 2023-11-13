using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto;

namespace YourRest.Application.UseCases
{
    public class GetAccommodationTypeListUseCase : IGetAccommodationTypeListUseCase
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        
        public GetAccommodationTypeListUseCase(IAccommodationTypeRepository accommodationRepository)
        {
            _accommodationTypeRepository = accommodationRepository;
        }

        public async Task<IEnumerable<AccommodationTypeDto>> Execute()
        {
            var accommodationTypes = await _accommodationTypeRepository.GetAllAsync();

            return accommodationTypes.Select(r => new AccommodationTypeDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }
    }
}
