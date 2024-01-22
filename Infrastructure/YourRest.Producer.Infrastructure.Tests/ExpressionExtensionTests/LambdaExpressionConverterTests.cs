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
        private static int cityId = 6;

        [Theory]
        [MemberData(nameof(IncludeFunctionDataData))]
        public void ExpressionConvertTest<TSource, TDestination>(TSource source, TDestination destination, Expression<Func<TSource, bool>> income, Expression<Func<TDestination, bool>> outcome)
        {
            // Act
            var qwe = income.ToEntityExpression<TSource, TDestination>();
            Debug.WriteLine("outcome: {0}, afterConvert: {1}", outcome.ToString(), qwe.ToString());
            Assert.Equal(outcome.ToString(), qwe.ToString());
        }

        public static IEnumerable<object[]> IncludeFunctionDataData => new List<object[]>
        {
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.Id == 5), (Expression<Func<Address, bool>>)((Address a) => a.Id == 5) },
            new object[] { new AddressDto(), new Address(), (Expression<Func<AddressDto, bool>>)((AddressDto a) => a.CityId == cityId), (Expression<Func<Address, bool>>)((Address a) => a.CityId == cityId) },
        };
    }
}
