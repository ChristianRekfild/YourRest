using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Application.Interfaces.Accommodation;

namespace YourRest.Application.UseCases.Accommodation
{
    public class FetchAccommodationsUseCase : IFetchAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationMapper _mapper;
        private readonly IUserRepository _userRepository;


        public FetchAccommodationsUseCase(
            IAccommodationRepository accommodationRepository,
            IAccommodationMapper mapper,
            IUserRepository userRepository
        ) {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<AccommodationExtendedDto>> ExecuteAsync(string sub, FetchAccommodationsViewModel viewModel, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(a => a.KeyCloakId == sub, cancellationToken);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(sub);
            }

            var domainFilterCriteria = _mapper.MapToFilterCriteria(viewModel);

            var hotels = await _accommodationRepository.GetHotelsByFilter(user.Id, domainFilterCriteria, cancellationToken);
            foreach (var hotel in hotels)
            {
                foreach (var userAccommodation in hotel.UserAccommodations)
                {
                    Console.WriteLine($"User ID: {user.Id}, User ID: {userAccommodation.UserId}");
                }
            }

            return hotels.Select(h => ConvertToDto(h)).ToList();
        }


        private AccommodationExtendedDto ConvertToDto(Domain.Entities.Accommodation accommodation)
        {
            return new AccommodationExtendedDto
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Description = accommodation.Description,
                Stars = accommodation.StarRating?.Stars,
                Address = accommodation.Address != null ? new AddressDto
                {
                    ZipCode = accommodation.Address.ZipCode,
                    Street = accommodation.Address.Street,
                    CityId = accommodation.Address.CityId
                } : null,
                AccommodationType = new AccommodationTypeDto
                {
                    Id = accommodation.AccommodationTypeId,
                    Name = accommodation.AccommodationType.Name
                },
                Rooms = accommodation.Rooms?.Select(r => new RoomWithIdDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    SquareInMeter = r.SquareInMeter,
                    RoomTypeId = r.RoomType.Id
                }).ToList() ?? new List<RoomWithIdDto>()
            };
        }
    }
}
