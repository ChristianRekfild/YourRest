using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.WebApi.Models;

namespace YourRest.WebApi.Mappers
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<AddressViewModel, AddressWithIdDto>()
                .ReverseMap();
        }
    }
}
