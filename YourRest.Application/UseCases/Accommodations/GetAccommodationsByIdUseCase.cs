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
using YourRest.Application.Interfaces.Room;
using YourRest.Application.UseCases.Room;
using YourRest.Application.Interfaces;

namespace YourRest.Application.UseCases.Accommodations
{
    public class GetAccommodationsByIdUseCase : IGetAccommodationsByIdUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;
        //private readonly IRoomRepository _roomRepository;


        public GetAccommodationsByIdUseCase(IAccommodationRepository accommodationRepository, IMapper mapper/*, IRoomRepository roomRepository*/)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
            //_roomRepository = roomRepository;
        }
        public async Task<AccommodationExtendedDto> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var accommodation = (await _accommodationRepository.GetWithIncludeAndTrackingAsync(a => a.Id == id, cancellationToken, include => include.StarRating)).FirstOrDefault();
            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id number {id} not found");
            }

            return _mapper.Map<AccommodationExtendedDto>(accommodation);
        }

    }
}
