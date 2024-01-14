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

            List<Domain.Entities.Room> romsUpdate = new List<Domain.Entities.Room>();
            AccommodationExtendedDto.Rooms.ForEach(r => romsUpdate.Add(_mapper.Map<Domain.Entities.Room>(r)));

            AccommodationStarRating sRating = new AccommodationStarRating();
            if (AccommodationExtendedDto.Stars != null)
            {
                sRating.Stars = (int)AccommodationExtendedDto.Stars;
            }
            sRating.AccommodationId = AccommodationExtendedDto.Id;

            var accommodationUpdate = new Domain.Entities.Accommodation
            {
                Id = AccommodationExtendedDto.Id,
                AddressId = AccommodationExtendedDto.Address.Id,
                Name = AccommodationExtendedDto.Name,
                AccommodationTypeId = AccommodationExtendedDto.AccommodationType.Id,
                Rooms = romsUpdate,
                Description = AccommodationExtendedDto.Description,
                StarRating = sRating
            };


            var accommodationToReturn = await _accommodationRepository.UpdateAsync(accommodationUpdate, cancellationToken: cancellationToken);


            List<RoomWithIdDto> romsUpdateToReturn = new List<RoomWithIdDto>();
            accommodationToReturn.Rooms.ToList().ForEach(r => romsUpdateToReturn.Add(_mapper.Map<RoomWithIdDto>(r)));

            var accommodationToReturnDto = new AccommodationExtendedDto()
            {
                Id = accommodationToReturn.Id,
                Address = _mapper.Map<AddressDto>(accommodationToReturn.Address),
                Name = AccommodationExtendedDto.Name,
                AccommodationType = AccommodationExtendedDto.AccommodationType,
                Rooms = romsUpdateToReturn,
                Description = accommodationToReturn.Description,
                Stars = accommodationToReturn.StarRating.Stars
            };

            return accommodationToReturnDto;
        }

    }
}
