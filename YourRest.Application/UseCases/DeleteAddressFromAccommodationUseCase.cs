using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class DeleteAddressFromAccommodationUseCase : IDeleteAddressFromAccommodationUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public DeleteAddressFromAccommodationUseCase(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public async Task<bool> ExecuteAsync(int accommodationId, int addressId)
        {
            var accommodations = await _accommodationRepository.GetWithIncludeAndTrackingAsync(
                a => a.Id == accommodationId,
                cancellationToken: default,
                a => a.Address
            );
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {accommodationId} not found");
            }
            
            if (accommodation.Address == null )
            {
                throw new ValidationException($"Address for accommodation with id {accommodationId} not exist");
            }

            if (accommodation.Address != null &&  accommodation.Address.Id != addressId)
            {
                throw new ValidationException($"No Address {addressId} for accommodation with id {accommodationId}");
            }

            accommodation.Address = null;
            await _accommodationRepository.SaveChangesAsync();
            
            return true;
        }
    }
}