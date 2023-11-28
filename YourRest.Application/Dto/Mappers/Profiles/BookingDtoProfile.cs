using AutoMapper;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    //public class BookingDtoProfile : Profile
    //{
    //    public BookingDtoProfile()
    //    {
            //CreateMap<Booking, BookingDto>()
            //.ForMember(target => target.StartDate, opt => opt.MapFrom(src => src.StartDate))
            //.ForMember(target => target.EndDate, opt => opt.MapFrom(src => src.EndDate))
            //.ForMember(target => target.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            //.ForMember(target => target.AdultNumber, opt => opt.MapFrom(src => src.AdultNumber))
            //.ForMember(target => target.ChildrenNumber, opt => opt.MapFrom(src => src.ChildrenNumber))
            //.ForPath(target => target.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
            //.ForPath(target => target.MiddleName, opt => opt.MapFrom(src => src.Customer.MiddleName))
            //.ForPath(target => target.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
            //.ForPath(target => target.DateOfBirth, opt => opt.MapFrom(src => src.Customer.DateOfBirth))
            //.ForPath(target => target.PassportNumber, opt => opt.MapFrom(src => src.Customer.PassportNumber))
            //.ForPath(target => target.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
            //.ForPath(target => target.Email, opt => opt.MapFrom(src => src.Customer.Email))
            //.ForPath(target => target.ExternalId, opt => opt.MapFrom(src => src.Customer.ExternalId))
            //.ForPath(target => target.SystemId, opt => opt.MapFrom(src => src.Customer.SystemId))
            //.ForMember(target => target.Rooms, opt => opt.MapFrom(src => src.Rooms))
            //.ReverseMap();

            //CreateMap<Booking, BookingWithIdDto>()
            //.ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            //.ReverseMap();

            //CreateMap<BookingDto, BookingWithIdDto>()
            //.ForMember(target => target.Id, opt => opt.Ignore())
            //.ReverseMap();
    //    }
    //}
}
