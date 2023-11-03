using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetRoomListUseCase : IGetRoomListUseCase
    {
        private readonly IRoomRepository _roomRepository;
        
        public GetRoomListUseCase(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomWithIdDto>> Execute(int accommodationId, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.FindAsync(t => t.AccommodationId == accommodationId, cancellationToken);

            return rooms.Select(r => new RoomWithIdDto   
            {
                Id = r.Id,
                Name = r.Name,
                SquareInMeter = r.SquareInMeter,
                RoomType = r.RoomType,
                Capacity = r.Capacity

            }).ToList();
        }
    }
}
