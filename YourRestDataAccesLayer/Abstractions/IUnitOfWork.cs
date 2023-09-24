namespace YourRestDataAccesLayer.Abstractions
{
    public interface IUnitOfWork
    {
        IAdditionalRoomServiceRepository AdditionalRoomServiceRepository { get;}
        IRoomRepository RoomRepository { get;}
        Task SaveChangesAsync();
    }
}
