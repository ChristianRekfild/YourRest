using YourRest.Application.Interfaces.Addresses;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Addresses
{
    public class RemoveAddressUseCase : IRemoveAddressUseCase
    {
        private readonly IAddressRepository addressRepository;

        public RemoveAddressUseCase(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public async Task ExecuteAsync(int Id, CancellationToken cancellationToken)
        {
            var address = (await addressRepository.FindAsync(a => a.Id == Id, cancellationToken)).FirstOrDefault();

            if (address == null)
            {
                throw new Exception($"Адрес с Id {Id} не найден.");
            }
                      
            await addressRepository.DeleteAsync(Id, true, cancellationToken);
        }
    }
}
