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
        public async Task<AccommodationExtendedDto> ExecuteAsync(AccommodationExtendedDto AccommodationExtendedDto, CancellationToken cancellationToken)
        {
            var accommodationToEdit = await _accommodationRepository.GetAsync(AccommodationExtendedDto.Id, cancellationToken);

            if (accommodationToEdit == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {AccommodationExtendedDto.Id} not found");
            }
            var accommodation = await _accommodationRepository.GetWithIncludeAndTrackingAsync(a => a.Id == AccommodationExtendedDto.Id, cancellationToken, include => include.StarRating , include => include.AccommodationType, include => include.Address, include => include.AccommodationFacilities);

            var acco
            //var accommodationToReturn = await _accommodationRepository.UpdateAsync(_mapper.Map<Accommodation>(AccommodationExtendedDto), cancellationToken: cancellationToken);

            //List<RoomWithIdDto> romsUpdateToReturn = new List<RoomWithIdDto>();
            //accommodationToReturn.Rooms.ToList().ForEach(r => romsUpdateToReturn.Add(_mapper.Map<RoomWithIdDto>(r)));

            //var accommodationToReturnDto = new AccommodationExtendedDto()
            //{
            //    Id = accommodationToReturn.Id,
            //    Address = _mapper.Map<AddressDto>(accommodationToReturn.Address),
            //    Name = AccommodationExtendedDto.Name,
            //    AccommodationType = AccommodationExtendedDto.AccommodationType,
            //    Rooms = romsUpdateToReturn,
            //    Description = accommodationToReturn.Description,
            //    Stars = accommodationToReturn.StarRating.Stars
            //};

            return _mapper.Map<AccommodationExtendedDto>(accommodationToReturn);
        }
    }
}
