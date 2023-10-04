using YourRest.Application.Dto;
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

        public async Task<IEnumerable<RoomDto>> Execute(int accommodationId)
        {
            var rooms = await _roomRepository.FindAsync(t => t.AccommodationId == accommodationId);

            return rooms.Select(r => new RoomDto   
            {
                Id = r.Id,
                Name = r.Name,
                SquareInMeter = r.SquareInMeter,
                AccomodationId = r.AccommodationId,     
                RoomType = r.RoomType,
                Capacity = r.Capacity

            }).ToList();
        
        }
    }
}
