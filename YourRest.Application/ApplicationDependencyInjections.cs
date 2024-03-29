﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Globalization;
using System.Reflection;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Validators;
using YourRest.Application.Dto.ViewModels;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.Age;
using YourRest.Application.Interfaces.Facility;
using YourRest.Application.Interfaces.HotelBooking;
using YourRest.Application.Interfaces.Photo;
using YourRest.Application.Interfaces.Room;
using YourRest.Application.Services;
using YourRest.Application.UseCases;
using YourRest.Application.UseCases.AgeRangeUseCases;
using YourRest.Application.UseCases.Facility;
using YourRest.Application.UseCases.Room;
using YourRest.Application.Interfaces.AccommodationFacility;
using YourRest.Application.UseCases.HotelBookingUseCase;
using YourRest.Application.UseCases.Photo;
using YourRest.Application.Interfaces.Addresses;
using YourRest.Application.UseCases.Addresses;

namespace YourRest.Application
{
    public static class ApplicationDependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Configure FluentValidation more information of configure here: https://github.com/SharpGrip/FluentValidation.AutoValidation
            services.AddFluentValidationAutoValidation(cfg =>
            {
                // Disable the built-in .NET model (data annotations) validation.
                cfg.DisableBuiltInModelValidation = true;
                // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
                cfg.ValidationStrategy = ValidationStrategy.Annotations;
                //[FromBody]
                cfg.EnableBodyBindingSourceAutomaticValidation = true;
                //[FromForm]
                cfg.EnableFormBindingSourceAutomaticValidation = true;
                //[FromQuery]
                cfg.EnableQueryBindingSourceAutomaticValidation = true;
                //[FromRoute]
                cfg.EnablePathBindingSourceAutomaticValidation = true;
                // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
                cfg.EnableCustomBindingSourceAutomaticValidation = true;
                // Replace the default result factory with a custom implementation.
                cfg.OverrideDefaultResultFactoryWith<YouRestResultFactory>();
            });
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
            services.AddValidatorsFromAssemblyContaining<RoomDtoValidator>();

            services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
            services.AddScoped<IGetCityByIdUseCase, GetCityByIdUseCase>();
            services.AddScoped<IGetCityListUseCase, GetCityListUseCase>();
            services.AddScoped<IGetRegionListUseCase, GetRegionListUseCase>();
            services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
            services.AddScoped<IAddAddressToAccommodationUseCase, AddAddressToAccommodationUseCase>();
            services.AddScoped<IGetCityByRegionIdUseCase, GetCityByRegionIdUseCase>();
            services.AddScoped<IGetCityByCountryIdUseCase, GetCityByCountryIdUseCase>();
            services.AddScoped<IGetRoomListUseCase, GetRoomListUseCase>();
            services.AddScoped<ICreateRoomUseCase, CreateRoomUseCase>();
            //Room
            services.AddScoped<IEditRoomUseCase, EditRoomUseCase>();
            services.AddScoped<IGetRoomByIdUseCase, GetRoomByIdUseCase>();
            services.AddScoped<IRemoveRoomUseCase, RemoveRoomUseCase>();
            services.AddScoped<IGetFacilitiesByRoomIdUseCase, GetRoomFacilitiesByRoomIdUseCase>();
            services.AddScoped<IAddRoomFacilityUseCase, AddRoomFacilityUseCase>();
            //RoomFacility
            services.AddScoped<IEditRoomFacilityUseCase, EditRoomFacilityUseCase>();
            services.AddScoped<IGetRoomFacilityByIdUseCase, GetRoomFacilityByIdUseCase>();
            services.AddScoped<IRemoveRoomFacilityUseCase, RemoveRoomFacilityUseCase>();
            services.AddScoped<IGetRoomTypeListUseCase, GetRoomTypeListUseCase>();
            services.AddScoped<IGetAllRoomFacilitiesUseCase, GetAllRoomFacilitiesUseCase>();
            //AccommodationFacility
            services.AddScoped<IAddAccommodationFacilityUseCase, AddAccommodationFacilityUseCase>();
            services.AddScoped<IRemoveAccommodationFacilityUseCase, RemoveAccommodationFacilityUseCase>();
            services.AddScoped<IGetAllAccommodationFacilitiesUseCase, GetAllAccommodationFacilitiesUseCase>();
            services.AddScoped<IGetAccommodationFacilityByAccommodationIdUseCase, GetAccommodationFacilityByAccommodationIdUseCase>();
            //AgeRange
            services.AddScoped<ICreateAgeRangeUseCase, CreateAgeRangeUseCase>();
            services.AddScoped<IEditAgeRangeUseCase, EditAgeRangeUseCase>();
            services.AddScoped<IGetAgeRangeByIdUseCase, GetAgeRangeByIdUseCase>();
            //HotelBooking
            services.AddScoped<ICreateBookingUseCase, CreateBookingUseCase>();
            services.AddScoped<IGetBookingDatesByRoomIdUseCase, GetBookingDatesByRoomIdUseCase>();
            services.AddScoped<IGetRoomsByCityAndBookingDatesUseCase, GetRoomsByCityAndBookingDatesUseCase>();
            services.AddScoped<IGetRoomsByAccommodationAndBookingDatesUseCase, GetRoomsByAccommodationAndBookingDatesUseCase>();

            services.AddScoped<IAccommodationMapper, AccommodationMapper>();
            services.AddScoped<IFetchAccommodationsUseCase, FetchAccommodationsUseCase>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccommodationPhotoUploadUseCase, AccommodationPhotoUploadUseCase>();
            services.AddScoped<IRoomPhotoUploadUseCase, RoomPhotoUploadUseCase>();
            services.AddScoped<ICreateAccommodationUseCase, CreateAccommodationUseCase>();
            services.AddScoped<IGetAccommodationTypeListUseCase, GetAccommodationTypeListUseCase>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserPhotoUploadUseCase, UserPhotoUploadUseCase>();
            services.AddScoped<ICityPhotoUploadUseCase, CityPhotoUploadUseCase>();
            services.AddScoped<IGetUserInfoUseCase, GetUserInfoUseCase>();
            services.AddScoped<IGetUserPhotosUseCase, GetUserPhotosUseCase>();
            //Address
            services.AddScoped<IAddAddressUseCase, AddAddressUseCase>();
            services.AddScoped<IEditAddressUseCase, EditAddressUseCase>();
            services.AddScoped<IGetAddressByCityIdUseCase, GetAddressByCityIdUseCase>();
            services.AddScoped<IGetAddressByIdUseCase, GetAddressByIdUseCase>();
            services.AddScoped<IGetAddressUseCase, GetAddressUseCase>();
            services.AddScoped<IRemoveAddressUseCase, RemoveAddressUseCase>();
            services.AddScoped<IDeleteAddressFromAccommodationUseCase, DeleteAddressFromAccommodationUseCase>();

            return services;
        }
    }
    internal class YouRestResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            return new BadRequestObjectResult(new ErrorViewModel() { Title = "Validation errors", ValidationErrors = validationProblemDetails?.Errors ?? new Dictionary<string, string[]>() });
        }
    }
}
