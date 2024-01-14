using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodation;
using YourRest.Application.Interfaces;
using YourRest.Application.Exceptions;
using YourRest.Domain.Models;

namespace YourRest.Application.UseCases.Accommodation
{
    public class EditAccommodationsUseCase : IEditAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public EditAccommodationsUseCase(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }
        public async Task<AccommodationExtendedDto> ExecuteAsync(AccommodationExtendedDto AccommodationExtendedDto, CancellationToken cancellationToken)
        {
            var accommodationToEdit = await _accommodationRepository.GetAsync(accommodationDto.Id, cancellationToken);

            if (accommodationToEdit == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {AccommodationExtendedDto.Id} not found");
            }

            var accommodationUpdate = new YourRest.Domain.Entities.Accommodation 
            { 
                Id = AccommodationExtendedDto.Id,
                AddressId = AccommodationExtendedDto.Address,

            };


            await _accommodationRepository.UpdateAsync(accommodationUpdate, cancellationToken: cancellationToken);
            
            return 
        }

    }
}
