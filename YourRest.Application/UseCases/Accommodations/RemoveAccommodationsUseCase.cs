using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodation;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Accommodations;

namespace YourRest.Application.UseCases.Accommodations
{
    public class RemoveAccommodationsUseCase : IRemoveAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public RemoveAccommodationsUseCase(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }
        public async Task ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetAsync(id, cancellationToken);
            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id number {id} not found");
            }
            await _accommodationRepository.DeleteAsync(id, cancellationToken: cancellationToken);
        }

    }
}
