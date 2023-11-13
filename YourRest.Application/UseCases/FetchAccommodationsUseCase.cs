using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class FetchAccommodationsUseCase : IFetchAccommodationsUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationMapper _mapper;

        public FetchAccommodationsUseCase(
            IAccommodationRepository accommodationRepository,
            IAccommodationMapper mapper
        ) {
            _accommodationRepository = accommodationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccommodationExtendedDto>> Execute(FetchAccommodationsViewModel viewModel)
        {
            var domainFilterCriteria = _mapper.MapToFilterCriteria(viewModel);

            var hotels = await _accommodationRepository.GetHotelsByFilter(domainFilterCriteria);
            return hotels.Select(h => ConvertToDto(h)).ToList();
        }


        private AccommodationExtendedDto ConvertToDto(Accommodation accommodation)
        {
            return new AccommodationExtendedDto
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Description = accommodation.Description,
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
                    RoomType = r.RoomType
                }).ToList() ?? new List<RoomWithIdDto>()
            };
        }
    }
}
