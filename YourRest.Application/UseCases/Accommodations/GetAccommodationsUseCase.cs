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
using YourRest.Application.Interfaces;

namespace YourRest.Application.UseCases.Accommodations
{
    public class GetAccommodationsUseCase : IGetAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IMapper _mapper;
        //private readonly IRoomRepository _roomRepository;


        public GetAccommodationsUseCase(IAccommodationRepository accommodationRepository, IMapper mapper/*, IRoomRepository roomRepository*/)
        {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
            //_roomRepository = roomRepository;
        }
        public async Task<IEnumerable<AccommodationExtendedDto>> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    var accList = await _accommodationRepository.GetAllAsync(cancellationToken);
        //    List<AccommodationExtendedDto> resultAccExtDtoList = new List<AccommodationExtendedDto>();
        //    foreach (var acc in accList)
        //    {
        //        AccommodationExtendedDto accExtDto = new AccommodationExtendedDto();
        //        accExtDto.Id = acc.Id;
        //        accExtDto.Name = acc.Name;
        //        accExtDto.Address = _mapper.Map<AddressDto>(acc.Address);
        //        List<RoomWithIdDto> roomsListWithIdDto = new List<RoomWithIdDto>();
        //        if (accExtDto.Rooms != null)
        //        {
        //            foreach (var room in accExtDto.Rooms)
        //            {
        //                RoomWithIdDto roomWithIdDto = new RoomWithIdDto();
        //                roomWithIdDto.Id = room.Id;
        //                roomWithIdDto.Name = room.Name;
        //                roomWithIdDto.Capacity = room.Capacity;
        //                roomWithIdDto.RoomTypeId = room.RoomTypeId;
        //                roomWithIdDto.SquareInMeter = room.SquareInMeter;

        //                roomsListWithIdDto.Add(roomWithIdDto);
        //                //roomsListWithIdDto.Add(_mapper.Map<RoomWithIdDto>(room));
        //            }
        //        }
        //        accExtDto.Rooms = roomsListWithIdDto;
        //        if (accExtDto.Stars != null)
        //        {
        //            accExtDto.Stars = acc.StarRating.Stars;
        //        }
        //        if (accExtDto.Description != null)
        //        {
        //            accExtDto.Description = acc.Description;
        //        }

        //        if (accExtDto.AccommodationType != null)
        //        {
        //            accExtDto.AccommodationType = _mapper.Map<AccommodationTypeDto>(acc.AccommodationType);
        //        }

        //        resultAccExtDtoList.Add(accExtDto);
        //    }
        //    return resultAccExtDtoList;
        //}

        {
            var accAll = await _accommodationRepository.GetAllAsync(cancellationToken);

            //foreach (var item in accAll)
            //{
            //    item.Rooms = (await _roomRepository.GetWithIncludeAsync(
            //    room => room.AccommodationId == item.Id,
            //    cancellationToken,
            //include => include.RoomType
            //    )).ToList();
            //}
            return _mapper.Map<IEnumerable<AccommodationExtendedDto>>(accAll);
        }
    }
}
