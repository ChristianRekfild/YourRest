using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.Producer.Infrastructure.ExpressionHelper;

namespace YourRest.Producer.Infrastructure.Tests.ExpressionExtensionTests
{
    public class LambdaExpressionConverterTests
    {
        private static AddressDto addressDto = new AddressDto { Id = 5 };
        private static AccommodationFacilityLinkDto accommodationFacilityDto = new AccommodationFacilityLinkDto { Id = 5 };
        private static int cityId = 6;

        private static readonly IMapper mapper;

        static LambdaExpressionConverterTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProducerInfrastructureMapper>());
            mapper = config.CreateMapper();
        }

        [Theory]
        [MemberData(nameof(ExpressionFunctionDataData))]
        public void ExpressionConvertTest<TSource, TDestination>(TSource source, TDestination destination, Expression<Func<TSource, bool>> income, Expression<Func<TDestination, bool>> outcome)
        {
            // Act
            var qwe = income.ToEntityExpression<TSource, TDestination>();
            Debug.WriteLine("outcome: {0}, afterConvert: {1}", outcome.ToString(), qwe.ToString());
            Assert.Equal(outcome.ToString(), qwe.ToString());
        }

        [Theory]
        [MemberData(nameof(ExpressionCallFunctionDataData))]
        public void ExpressionConvertCallMethodTest<TSource, TDestination>(TSource source, TDestination destination, Expression<Func<TSource, bool>> income, Expression<Func<TDestination, bool>> outcome)
        {
            // Act
            var qwe = income.ToEntityExpression<TSource, TDestination>();
            Debug.WriteLine("outcome: {0}, afterConvert: {1}", outcome.ToString(), qwe.ToString());
            Assert.Equal(outcome.ToString(), qwe.ToString());
        }


        public static IEnumerable<object[]> ExpressionCallFunctionDataData => new List<object[]>
        {
            new object[] {
                new AccommodationDto(),
                new Accommodation(),
                (Expression<Func<AccommodationDto, bool>>)((AccommodationDto a) => a.AccommodationFacilities.Select(f => f.Id).Contains(accommodationFacilityDto.Id)),
                (Expression<Func<Accommodation, bool>>)((Accommodation a) => a.AccommodationFacilities.Select(f => f.Id).Contains(accommodationFacilityDto.Id)) },
            //new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId == cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId == cityId) },
        };

        public static IEnumerable<object[]> ExpressionFunctionDataData => new List<object[]>
        {
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id == 5), (Expression<Func<Address, bool>>)((Address a) => a.Id == 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId == cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId == cityId) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id > 5), (Expression<Func<Address, bool>>)((Address a) => a.Id > 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId > cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId > cityId) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id >= 5), (Expression<Func<Address, bool>>)((Address a) => a.Id >= 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId >= cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId >= cityId) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id < 5), (Expression<Func<Address, bool>>)((Address a) => a.Id < 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId < cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId < cityId) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id <= 5), (Expression<Func<Address, bool>>)((Address a) => a.Id <= 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId <= cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId <= cityId) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id < 5 || a.CityId > 5), (Expression<Func<Address, bool>>)((Address a) => a.Id < 5 || a.CityId > 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId < cityId && a.Id > 5), (Expression<Func<Address, bool>>)((Address a) => a.CityId < cityId && a.Id > 5) },
        };
    }
}
