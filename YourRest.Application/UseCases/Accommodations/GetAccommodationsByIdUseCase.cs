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
        private readonly IGetRoomListUseCase _getRoomListUseCase;


        public GetAccommodationsByIdUseCase(IAccommodationRepository accommodationRepository, IMapper mapper, IGetRoomListUseCase getRoomListUseCase)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
            _getRoomListUseCase = getRoomListUseCase;
        }
        public async Task<AccommodationExtendedDto> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetAsync(id, cancellationToken);
            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id number {id} not found");
            }
            var accommodationExtendedDto = _mapper.Map<AccommodationExtendedDto>(accommodation);
            accommodationExtendedDto.Rooms = (await _getRoomListUseCase.Execute(accommodation.Id, cancellationToken)).ToList();
            return accommodationExtendedDto;
        }

    }
}
