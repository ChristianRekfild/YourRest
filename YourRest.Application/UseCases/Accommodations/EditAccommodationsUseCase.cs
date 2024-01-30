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

        public EditAccommodationsUseCase(IAccommodationRepository accommodationRepository, IMapper mapper/*, IAccommodationStarRatingRepository starRatingRepository*/)
        {
            _accommodationRepository = accommodationRepository;
        }
        public async Task<AccommodationDto> ExecuteAsync(EditAccommodationDto editAccommodationDto, CancellationToken cancellationToken)
        {
            var accommodationInDb = (await _accommodationRepository.GetWithIncludeAndTrackingAsync(
                a => a.Id == editAccommodationDto.Id,
                cancellationToken,
                include => include.StarRating,
                include => include.Rooms,
                include => include.AccommodationType
                )).FirstOrDefault();

            if (accommodationInDb == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {editAccommodationDto.Id} not found");
            }
            accommodationInDb.Description = editAccommodationDto.Description;
            accommodationInDb.Name = editAccommodationDto.Name;
            accommodationInDb.AccommodationTypeId = editAccommodationDto.AccommodationTypeId;
            if (accommodationInDb.StarRating == null)
            {
                accommodationInDb.StarRating = new AccommodationStarRating()
                {
                    Stars = (int)editAccommodationDto.Stars,
                    Accommodation = accommodationInDb,
                    AccommodationId = accommodationInDb.Id
                };
            }
            else
            {
                accommodationInDb.StarRating.Stars = (int)editAccommodationDto.Stars;
            }


            var accommodationToReturn = await _accommodationRepository.UpdateAsync((Accommodation)accommodationInDb, cancellationToken: cancellationToken);

            AccommodationDto accommodationDtoToReturn = new AccommodationDto()
            {
                Id = accommodationToReturn.Id,
                Name = accommodationToReturn.Name,
                Stars = accommodationToReturn.StarRating.Stars,
                AccommodationType = new AccommodationTypeDto()
                {
                    Id = accommodationToReturn.AccommodationType.Id,
                    Name = accommodationToReturn.AccommodationType.Name
                },
                Description = accommodationToReturn.Description
            };
            return accommodationDtoToReturn;
        }
    }
}
