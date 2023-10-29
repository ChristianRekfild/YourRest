using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetRoomTypeListUseCase : IGetRoomTypeListUseCase
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        
        public GetRoomTypeListUseCase(IRoomTypeRepository roomRepository)
        {
            _roomTypeRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomTypeDto>> Execute()
        {
            var roomTypes = await _roomTypeRepository.GetAllAsync();

            return roomTypes.Select(r => new RoomTypeDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }
    }
}
