using YourRest.Application.Dto;
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

        public async Task<IEnumerable<RoomWithIdDto>> Execute(int accommodationId)
        {
            var rooms = await _roomRepository.FindAsync(t => t.AccommodationId == accommodationId);

            return rooms.Select(r => new RoomWithIdDto   
            {
                Id = r.Id,
                Name = r.Name,
                SquareInMeter = r.SquareInMeter,
                AccommodationId = r.AccommodationId,     
                RoomType = r.RoomType,
                Capacity = r.Capacity

            }).ToList();
        }
    }
}
