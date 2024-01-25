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
        private readonly IAccommodationStarRatingRepository _starRatingRepository;

        public EditAccommodationsUseCase(IAccommodationRepository accommodationRepository, IMapper mapper, IAccommodationStarRatingRepository starRatingRepository)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
            _starRatingRepository = starRatingRepository;
        }
        public async Task<AccommodationExtendedDto> ExecuteAsync(EditAccommodationDto editAccommodationDto, CancellationToken cancellationToken)
        {
            var accommodationInDb = (await _accommodationRepository.GetWithIncludeAndTrackingAsync(
                a => a.Id == editAccommodationDto.Id,
                cancellationToken,
                include => include.StarRating,EDI 
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

            AccommodationStarRating star = new AccommodationStarRating();
            try
            {
                star = await _starRatingRepository.GetAsync(accommodationInDb.StarRating.Id, cancellationToken);
                star.Stars = (int)editAccommodationDto.Stars;
                await _starRatingRepository.UpdateAsync(star, true, cancellationToken);
            }
            catch (NullReferenceException err)
            {
                star.Stars = (int)editAccommodationDto.Stars;
                star.AccommodationId = editAccommodationDto.Id;
                await _starRatingRepository.AddAsync(star, true, cancellationToken);
            }

            accommodationInDb.StarRating = star;

            var accommodationToReturn = await _accommodationRepository.UpdateAsync((Accommodation)accommodationInDb, cancellationToken: cancellationToken);

            return _mapper.Map<AccommodationExtendedDto>(accommodationToReturn);
        }
    }
}
