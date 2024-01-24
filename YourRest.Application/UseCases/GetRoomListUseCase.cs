using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetRoomListUseCase : IGetRoomListUseCase
    {
        private readonly IRoomRepository _roomRepository;
        
        public GetRoomListUseCase(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomWithIdDto>> ExecuteAsync(int accommodationId, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.GetWithIncludeAsync(
                room => room.AccommodationId == accommodationId,
                cancellationToken,
            include => include.RoomType
                );

            return rooms.Select(r => new RoomWithIdDto   
            {
                Id = r.Id,
                Name = r.Name,
                SquareInMeter = r.SquareInMeter,
                RoomTypeId = r.RoomTypeId,
                Capacity = r.Capacity

            }).ToList();
        }
    }
}
