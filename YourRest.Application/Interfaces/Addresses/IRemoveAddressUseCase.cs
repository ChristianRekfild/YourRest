namespace YourRest.Application.Interfaces.Addresses
{
    public interface IRemoveAddressUseCase
    {
        Task ExecuteAsync(int Id, CancellationToken cancellationToken);
    }
}
