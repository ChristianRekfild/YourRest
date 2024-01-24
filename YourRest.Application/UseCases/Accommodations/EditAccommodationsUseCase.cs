using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodations;
using YourRest.Application.Exceptions;
using YourRest.Domain.Models;
using System.Xml.Linq;
using AutoMapper;
using YourRest.Application.Interfaces.Accommodations;

namespace YourRest.Application.UseCases.Accommodations
{
    public class EditAccommodationsUseCase : IEditAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;

        public EditAccommodationsUseCase(IAccommodationRepository accommodationRepository, IMapper mapper)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
        }
        public async Task<AccommodationExtendedDto> ExecuteAsync(CreateAccommodationDto accommodationChenges, int id, CancellationToken cancellationToken)
        {
            var accommodationInDb = (await _accommodationRepository.GetWithIncludeAndTrackingAsync(
                a => a.Id == id,
                cancellationToken,
                include => include.StarRating,
                include => include.Rooms,
                include => include.UserAccommodations,
                include => include.AccommodationType,
                include => include.AccommodationTypeId
                )).First();

            if (accommodationInDb == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {id} not found");
            }
            accommodationInDb.Description = accommodationChenges.Description;
            accommodationInDb.Name = accommodationChenges.Name;
            accommodationInDb.AccommodationTypeId = accommodationChenges.AccommodationTypeId;

            accommodationInDb.StarRating.Stars = (int)accommodationChenges.Stars;

            var accommodationToReturn = await _accommodationRepository.UpdateAsync(_mapper.Map<Accommodation>(accommodationInDb), cancellationToken: cancellationToken);


            return _mapper.Map<AccommodationExtendedDto>(accommodationToReturn);
        }
    }
}
