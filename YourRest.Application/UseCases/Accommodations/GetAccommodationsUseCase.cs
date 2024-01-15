using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodations;
using YourRest.Application.Exceptions;
using AutoMapper;

namespace YourRest.Application.UseCases.Accommodations
{
    public class GetAccommodationsUseCase : IGetAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;


        public GetAccommodationsUseCase(IAccommodationRepository accommodationRepository, IMapper mapper)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AccommodationExtendedDto>> ExecuteAsync(CancellationToken cancellationToken)
           => _mapper.Map<IEnumerable<AccommodationExtendedDto>>(await _accommodationRepository.GetAllAsync(cancellationToken));      
    }
}
