using AutoMapper;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Room
{
    public class GetRoomByIdUseCase : IGetRoomByIdUseCase
    {
        private readonly IRoomRepository roomRepository;
        private readonly IMapper mapper;

        public GetRoomByIdUseCase(IRoomRepository roomRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.mapper = mapper;
        }
        public async Task<RoomExtendedDto> ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetAsync(id, cancellationToken: cancellationToken);
            if (room == null)
            {
                throw new EntityNotFoundException($"Room with Id:{id} not found");
            }
            return mapper.Map<RoomExtendedDto>(room);
        }
    }
}
