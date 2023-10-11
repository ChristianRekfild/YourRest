using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IRemoveRoomFacilityUseCase
    {
        Task ExecuteAsync(int id);
    }
}
