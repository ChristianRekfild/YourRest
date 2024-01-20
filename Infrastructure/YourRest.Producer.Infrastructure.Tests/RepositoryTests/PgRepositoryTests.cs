using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Producer.Infrastructure.Tests.RepositoryTests
{
    public class PgRepositoryTests
    {
        [Fact]
        public void ExpressionConverTests()
        {
            // Arrange 
            Expression<Func<One, bool>> expression = one => one.Id < 3;
            List<Two> testCollection = new List<Two> {
                new Two { Id = 1,  Name = "One" },
                new Two { Id = 2,  Name = "Two" },
                new Two { Id = 3,  Name = "Three" },
                new Two { Id = 4,  Name = "Four" },
                new Two { Id = 5,  Name = "Five" },
            };

            // Act
            var result = testCollection.AsQueryable().Where(ChangeType(expression)).ToArray();

            Assert.NotEmpty(result);
        }

        private Expression<Func<Two, bool>> ChangeType(Expression<Func<One, bool>> expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                throw new NotSupportedException();
            }

            Expression<Func<Two, bool>> _expression = one => one.Id < 3;
            return _expression;
        }

        public static IEnumerable<object[]> objects
            => new List<object[]> {
                new[] { new Two { Id = 1,  Name = "One" } },
                new[] { new Two { Id = 2,  Name = "Two" } },
                new[] { new Two { Id = 3,  Name = "Three" } },
                new[] { new Two { Id = 4,  Name = "Four" } },
                new[] { new Two { Id = 5,  Name = "Five" } },
            };

        class One
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class Two
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
