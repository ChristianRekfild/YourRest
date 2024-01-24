using System.Diagnostics;
using System.Linq.Expressions;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.Producer.Infrastructure.ExpressionHelper;

namespace YourRest.Producer.Infrastructure.Tests.ExpressionExtensionTests
{
    public class LambdaExpressionExtensionIncludeTests
    {
        [Theory]
        [MemberData(nameof(IncludeFunctionDataData))]
        public void ConvertTest<TSource, TDestination>(TSource source, TDestination destination, Expression<Func<TSource, object>> income, Expression<Func<TDestination, object>> outcome)
        {
            // Act
            var qwe = income.ToEntityExpression<TSource, TDestination>();
            Debug.WriteLine("outcome: {0}, afterConvert: {1}", outcome.ToString(), qwe.ToString());
            Assert.Equal(outcome.ToString(), qwe.ToString());
        }
        
        public static IEnumerable<object[]> IncludeFunctionDataData => new List<object[]>
        {
            new object[] { new AccommodationDto(), new Accommodation(), (Expression<Func<AccommodationDto, object>>)((AccommodationDto a) => a.Address), (Expression<Func<Accommodation, object>>)((Accommodation a) => a.Address) },
        };
    }
}
