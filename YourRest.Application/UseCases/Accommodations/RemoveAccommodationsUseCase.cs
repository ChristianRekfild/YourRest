using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodations;
using YourRest.Application.Exceptions;

namespace YourRest.Application.UseCases.Accommodations
{
    public class RemoveAccommodationsUseCase : IRemoveAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccommodationStarRatingRepository _accommodationStarRatingRepository;

        public RemoveAccommodationsUseCase(
            IAccommodationRepository accommodationRepository,
            IRoomRepository roomRepository,
            IAccommodationStarRatingRepository accommodationStarRatingRepository)
        {
            _accommodationRepository = accommodationRepository;
            _roomRepository = roomRepository;
            _accommodationStarRatingRepository = accommodationStarRatingRepository;
        }
        public async Task ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var accommodation = (await _accommodationRepository.GetWithIncludeAndTrackingAsync(a => a.Id == id, cancellationToken, include => include.StarRating , include => include.Rooms)).First();
            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id number {id} not found");
            }
            int[] accRoomsId = new int[accommodation.Rooms.Count];            
            int i = 0;

            foreach (var room in accommodation.Rooms)
            {
                accRoomsId[i] = room.Id;
                i++;
            }

            foreach (var roomId in accRoomsId)
            {
                await _roomRepository.DeleteAsync(roomId, cancellationToken: cancellationToken);
            }


            await _accommodationStarRatingRepository.DeleteAsync(accommodation.StarRating.Id, cancellationToken: cancellationToken);
            await _accommodationRepository.DeleteAsync(id, cancellationToken: cancellationToken);
        }

    }
}
